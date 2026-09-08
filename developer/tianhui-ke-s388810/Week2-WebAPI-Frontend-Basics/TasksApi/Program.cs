using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TasksDbContext>();
    db.Database.EnsureCreated();
}

app.UseCors();

// GET all tasks
app.MapGet("/api/tasks", async (TasksDbContext db) =>
    await db.Tasks.OrderBy(t => t.Id).ToListAsync());

// GET single task
app.MapGet("/api/tasks/{id}", async (int id, TasksDbContext db) =>
    await db.Tasks.FindAsync(id) is { } task
        ? Results.Ok(task)
        : Results.NotFound(new { error = $"Task {id} not found." }));

// POST create task
app.MapPost("/api/tasks", async (TaskItem input, TasksDbContext db) =>
{
    // Validate at the API boundary — never trust the client to have already
    // validated. A missing/blank title here is a 400, not a 500 from a
    // NOT NULL constraint violation further down.
    if (string.IsNullOrWhiteSpace(input.Title))
    {
        return Results.BadRequest(new { error = "Title is required." });
    }

    var task = new TaskItem { Title = input.Title.Trim(), IsDone = false };
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tasks/{task.Id}", task);
});

// PUT update task (mark done / rename)
app.MapPut("/api/tasks/{id}", async (int id, TaskItem input, TasksDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null)
    {
        return Results.NotFound(new { error = $"Task {id} not found." });
    }

    if (string.IsNullOrWhiteSpace(input.Title))
    {
        return Results.BadRequest(new { error = "Title cannot be empty." });
    }

    task.Title = input.Title.Trim();
    task.IsDone = input.IsDone;
    await db.SaveChangesAsync();
    return Results.Ok(task);
});

// DELETE task
app.MapDelete("/api/tasks/{id}", async (int id, TasksDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null)
    {
        return Results.NotFound(new { error = $"Task {id} not found." });
    }

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsDone { get; set; }
}

class TasksDbContext : DbContext
{
    public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options) { }
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
}
