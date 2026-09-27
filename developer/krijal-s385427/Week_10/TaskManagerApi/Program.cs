using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagerApi.Data;
using TaskManagerApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog and Seq.
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

// Catch and log unhandled exceptions.
app.UseMiddleware<GlobalExceptionMiddleware>();

// Record structured HTTP request logs.
app.UseSerilogRequestLogging();

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
app.MapHealthChecks("/health");

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation(
        "Task Manager API root endpoint was accessed");

    return new
    {
        message = "Task Manager API is running"
    };
});

app.Run();