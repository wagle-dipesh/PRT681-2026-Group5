using Microsoft.EntityFrameworkCore;
using TheaterAdminWebApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// SQLite stand-in for SQL Express — same reasoning as the Week 2 MVC version (SQL Express
// is Windows-only, this was built on macOS). See README.md.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=theateradmin_api.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Serve the jQuery client from wwwroot at the site root, so the browser can call the
// API on the same origin (no CORS configuration needed).
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.Run();
