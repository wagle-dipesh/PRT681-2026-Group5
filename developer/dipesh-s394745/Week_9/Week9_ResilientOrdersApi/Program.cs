using ElmahCore.Mvc;
using Serilog;
using Week9_ResilientOrdersApi.Activities;
using Week9_ResilientOrdersApi.Models;
using Week9_ResilientOrdersApi.Workflows;

var builder = WebApplication.CreateBuilder(args);

// ---- Structured logging: Serilog -> Console + Seq -----------------------
// Every log call below (Log.Information, ILogger<T>, etc.) is a structured
// event with named properties (e.g. {OrderId}), not just a text string.
// Seq lets us search and filter on those properties later, e.g.
// "OrderId = 3", instead of grepping through plain text log files.
var seqUrl = builder.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341";
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq(seqUrl)
    .CreateLogger();
builder.Host.UseSerilog();

// ---- Exception tracking: ELMAH -------------------------------------------
// ElmahCore automatically records every unhandled exception (with the full
// stack trace and request details) and exposes them at /elmah, without us
// having to write any try/catch blocks ourselves.
builder.Services.AddElmah();

// ---- Health checks (used by the Docker HEALTHCHECK instruction) ---------
builder.Services.AddHealthChecks();

// ---- App services ----------------------------------------------------
builder.Services.AddSingleton<OrderStore>();
builder.Services.AddSingleton<EmailActivities>();
builder.Services.AddSingleton<TemporalClientProvider>();
builder.Services.AddHostedService<TemporalWorkerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging(); // logs every request (method, path, status, timing) to Seq
app.UseElmah();                 // catches unhandled exceptions for the /elmah dashboard

app.MapHealthChecks("/health");

// ---- Orders endpoints --------------------------------------------------

app.MapGet("/api/orders", (OrderStore store) => store.GetAll())
    .WithName("GetOrders");

app.MapGet("/api/orders/{id:int}", (int id, OrderStore store) =>
    store.GetById(id) is { } order ? Results.Ok(order) : Results.NotFound())
    .WithName("GetOrderById");

app.MapPost("/api/orders", async (
    OrderRequest request,
    OrderStore store,
    TemporalClientProvider clientProvider,
    ILogger<Program> logger) =>
{
    var errors = new List<string>();
    if (string.IsNullOrWhiteSpace(request.CustomerName)) errors.Add("Customer name is required.");
    if (string.IsNullOrWhiteSpace(request.CustomerEmail) || !request.CustomerEmail.Contains('@'))
        errors.Add("A valid customer email is required.");
    if (string.IsNullOrWhiteSpace(request.Product)) errors.Add("Product is required.");
    if (request.Quantity <= 0) errors.Add("Quantity must be at least 1.");
    if (errors.Count > 0) return Results.BadRequest(new { errors });

    var order = store.Add(request);
    logger.LogInformation(
        "Order {OrderId} created for {CustomerEmail}: {Quantity} x {Product}",
        order.Id, order.CustomerEmail, order.Quantity, order.Product);

    // Don't make the customer wait for the email to actually send - hand it
    // off to a durable Temporal workflow and respond immediately. Even if
    // this API process restarts a second later, Temporal remembers the
    // workflow was in progress and keeps driving it to completion.
    var client = await clientProvider.GetClientAsync();
    var handle = await client.StartWorkflowAsync(
        (OrderConfirmationWorkflow wf) => wf.RunAsync(new OrderConfirmationInput
        {
            OrderId = order.Id,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            Product = order.Product,
            Quantity = order.Quantity,
        }),
        new(id: $"order-confirmation-{order.Id}", taskQueue: TemporalWorkerService.TaskQueue));

    logger.LogInformation("Started Temporal workflow {WorkflowId} for Order {OrderId}", handle.Id, order.Id);

    return Results.Accepted($"/api/orders/{order.Id}", new { order, workflowId = handle.Id });
})
.WithName("CreateOrder");

// A deliberate failure endpoint so we can demonstrate ELMAH + Seq catching
// and recording a real unhandled exception.
app.MapGet("/throw", () =>
{
    throw new InvalidOperationException("Simulated failure for the ELMAH + Seq demo.");
});

app.Run();
