using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagerApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure structured logging.
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

// Records structured information about every HTTP request.
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
}

app.UseCors("AllowReactClient");

app.MapControllers();

app.MapGet("/", (ILogger<Program> logger) =>
{
    logger.LogInformation(
        "Task Manager API health endpoint was accessed");

    return new
    {
        message = "Task Manager API is running"
    };
});

app.Run();