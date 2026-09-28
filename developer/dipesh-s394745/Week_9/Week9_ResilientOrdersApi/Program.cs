using ElmahCore.Mvc;
using Serilog;
using Temporalio.Client;
using Temporalio.Worker;
using Week9_ResilientOrdersApi.Activities;
using Week9_ResilientOrdersApi.Models;
using Week9_ResilientOrdersApi.Workflows;

var builder = WebApplication.CreateBuilder(args);

// log to console and Seq
var seqUrl = builder.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341";
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Seq(seqUrl)
    .CreateLogger();
builder.Host.UseSerilog();

// ELMAH catches unhandled exceptions automatically
builder.Services.AddElmah();

// used by the Docker health check
builder.Services.AddHealthChecks();

builder.Services.AddSingleton<OrderStore>();
builder.Services.AddSingleton<EmailActivities>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();
app.UseElmah();

app.MapHealthChecks("/health");

// ---- Temporal setup -----------------------------------------------------
// Connect to the Temporal server, then start a worker that runs our
// workflow + activity whenever a new order comes in.
const string taskQueue = "orders-task-queue";
var temporalHostPort = app.Configuration["Temporal:HostPort"] ?? "localhost:7233";

TemporalClient temporalClient = null!;
for (var attempt = 1; ; attempt++)
{
    try
    {
        temporalClient = await TemporalClient.ConnectAsync(new(temporalHostPort));
        app.Logger.LogInformation("Connected to Temporal at {HostPort}", temporalHostPort);
        break;
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "Temporal not ready yet (attempt {Attempt}), retrying in 5s", attempt);
        await Task.Delay(TimeSpan.FromSeconds(5));
    }
}

var emailActivities = app.Services.GetRequiredService<EmailActivities>();
var worker = new TemporalWorker(
    temporalClient,
    new TemporalWorkerOptions(taskQueue)
        .AddAllActivities(emailActivities)
        .AddWorkflow<OrderConfirmationWorkflow>());

// run the worker for the lifetime of the app, in the background
_ = worker.ExecuteAsync(app.Lifetime.ApplicationStopping);

// ---- Orders endpoints --------------------------------------------------

app.MapGet("/api/orders", (OrderStore store) => store.GetAll());

app.MapGet("/api/orders/{id:int}", (int id, OrderStore store) =>
    store.GetById(id) is { } order ? Results.Ok(order) : Results.NotFound());

app.MapPost("/api/orders", async (OrderRequest request, OrderStore store, ILogger<Program> logger) =>
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

    // start the confirmation email as a Temporal workflow instead of sending it here directly
    var handle = await temporalClient.StartWorkflowAsync(
        (OrderConfirmationWorkflow wf) => wf.RunAsync(new OrderConfirmationInput
        {
            OrderId = order.Id,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            Product = order.Product,
            Quantity = order.Quantity,
        }),
        new(id: $"order-confirmation-{order.Id}", taskQueue: taskQueue));

    logger.LogInformation("Started Temporal workflow {WorkflowId} for Order {OrderId}", handle.Id, order.Id);

    return Results.Accepted($"/api/orders/{order.Id}", new { order, workflowId = handle.Id });
});

// test endpoint to trigger ELMAH
app.MapGet("/throw", () =>
{
    throw new InvalidOperationException("Simulated failure for the ELMAH + Seq demo.");
});

app.Run();
