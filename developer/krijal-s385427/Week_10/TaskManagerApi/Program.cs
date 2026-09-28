using Microsoft.EntityFrameworkCore;
using Serilog;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;
using TaskManagerApi.Data;
using TaskManagerApi.Middleware;
using TaskManagerApi.Models;
using TaskManagerApi.Services;
using TaskManagerApi.Settings;
using TaskManagerApi.Workflows;

var builder = WebApplication.CreateBuilder(args);

// Configure structured logging with Serilog and Seq.
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Application", "TaskManagerApi")
        .WriteTo.Console()
        .WriteTo.Seq(
            context.Configuration["Seq:ServerUrl"]
            ?? "http://localhost:5341");
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

// Configure MailKit email service.
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("Email"));

builder.Services.AddScoped<IEmailService, EmailService>();

// Configure Temporal.
var temporalTargetHost =
    builder.Configuration["Temporal:TargetHost"]
    ?? "localhost:7233";

var temporalNamespace =
    builder.Configuration["Temporal:Namespace"]
    ?? "default";

var temporalTaskQueue =
    builder.Configuration["Temporal:TaskQueue"]
    ?? "task-manager-email";

builder.Services
    .AddTemporalClient(options =>
    {
        options.TargetHost = temporalTargetHost;
        options.Namespace = temporalNamespace;
    })
    .AddHostedTemporalWorker(temporalTaskQueue)
    .AddWorkflow<EmailWorkflow>()
    .AddScopedActivities<EmailActivities>();

// Configure Entity Framework Core and SQL Server.
builder.Services.AddDbContext<TaskManagerContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "TaskManagerConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null);
        }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactClient", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Handle and log unhandled exceptions.
app.UseMiddleware<GlobalExceptionMiddleware>();

// Record structured HTTP request logs.
app.UseSerilogRequestLogging();

// Create the database and Tasks table for a new container.
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider
        .GetRequiredService<TaskManagerContext>();

    await database.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Development-only endpoint for testing exception logging.
    app.MapGet("/test-error", () =>
    {
        throw new InvalidOperationException(
            "This is a test exception for Seq.");
    });
}

app.UseCors("AllowReactClient");

app.MapControllers();

// Health-check endpoint.
app.MapHealthChecks("/health");

// Root endpoint.
app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation(
        "Task Manager API root endpoint was accessed");

    return new
    {
        message = "Task Manager API is running"
    };
});

// Direct MailKit email endpoint.
app.MapPost(
    "/api/email/send",
    async (
        EmailRequest request,
        IEmailService emailService) =>
    {
        if (string.IsNullOrWhiteSpace(request.Recipient) ||
            string.IsNullOrWhiteSpace(request.Subject))
        {
            return Results.BadRequest(new
            {
                message = "Recipient and subject are required."
            });
        }

        await emailService.SendEmailAsync(
            request.Recipient,
            request.Subject,
            request.Body);

        return Results.Accepted(value: new
        {
            message = "Email was sent successfully."
        });
    });

// Durable Temporal email workflow endpoint.
app.MapPost(
    "/api/email/workflow",
    async (
        EmailRequest request,
        ITemporalClient temporalClient) =>
    {
        if (string.IsNullOrWhiteSpace(request.Recipient) ||
            string.IsNullOrWhiteSpace(request.Subject))
        {
            return Results.BadRequest(new
            {
                message = "Recipient and subject are required."
            });
        }

        var workflowId = $"email-{Guid.NewGuid():N}";

        await temporalClient.StartWorkflowAsync(
            (EmailWorkflow workflow) =>
                workflow.RunAsync(request),
            new WorkflowOptions(
                id: workflowId,
                taskQueue: temporalTaskQueue));

        return Results.Accepted(value: new
        {
            message = "Email workflow started.",
            workflowId
        });
    });

app.Run();