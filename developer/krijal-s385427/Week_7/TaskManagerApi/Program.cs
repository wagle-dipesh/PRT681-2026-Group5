using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add API controller support.
builder.Services.AddControllers();

// Add OpenAPI documentation.
builder.Services.AddOpenApi();

// Configure Entity Framework Core and SQL Server.
builder.Services.AddDbContext<TaskManagerContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TaskManagerConnection")));

// Allow the React frontend to communicate with this API.
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

// Enable OpenAPI during development.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Apply the CORS policy.
app.UseCors("AllowReactClient");

// Connect attribute-routed API controllers.
app.MapControllers();

// Simple endpoint for checking whether the API is running.
app.MapGet("/", () => new
{
    message = "Task Manager API is running"
});

app.Run();