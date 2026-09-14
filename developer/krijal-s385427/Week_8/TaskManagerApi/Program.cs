using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TaskManagerContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TaskManagerConnection"),
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

// Create the database and Tasks table when using a new container.
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

app.MapGet("/", () => new
{
    message = "Task Manager API is running"
});

app.Run();