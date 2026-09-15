using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TasksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Server=localhost,1433;Database=WeeklyTasksDB;User Id=sa;Password=Passw0rd123;TrustServerCertificate=True;"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TasksDbContext>();
    // EnsureCreated() creates the database/table only if they don't already exist —
    // this reuses the same WeeklyTasksDB / Tasks table the Week 4 console app created,
    // rather than starting from a fresh database (this is meant to be the same project,
    // upgraded, not a new one).
    db.Database.EnsureCreated();
}

app.UseCors();

app.MapGet("/api/tasks", async (TasksDbContext db) =>
    await db.Tasks.OrderBy(t => t.Id).ToListAsync());

app.MapGet("/api/tasks/{id}", async (int id, TasksDbContext db) =>
    await db.Tasks.FindAsync(id) is { } task
        ? Results.Ok(task)
        : Results.NotFound(new { error = $"Task {id} not found." }));

app.MapPost("/api/tasks", async (TaskItem input, TasksDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(input.Title))
    {
        return Results.BadRequest(new { error = "Title is required." });
    }

    var task = new TaskItem { Title = input.Title.Trim(), IsDone = false };
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tasks/{task.Id}", task);
});

app.MapPut("/api/tasks/{id}", async (int id, TaskItem input, TasksDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound(new { error = $"Task {id} not found." });

    if (string.IsNullOrWhiteSpace(input.Title))
    {
        return Results.BadRequest(new { error = "Title cannot be empty." });
    }

    task.Title = input.Title.Trim();
    task.IsDone = input.IsDone;
    await db.SaveChangesAsync();
    return Results.Ok(task);
});

app.MapDelete("/api/tasks/{id}", async (int id, TasksDbContext db) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound(new { error = $"Task {id} not found." });

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

    // Explicitly mapped to the "Tasks" table with matching column names, since this table
    // already exists (created by the Week 4 console app) rather than being created fresh here.
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.ToTable("Tasks");
            entity.Property(t => t.Title).HasColumnName("Title");
            entity.Property(t => t.IsDone).HasColumnName("IsDone");
        });
    }
}
