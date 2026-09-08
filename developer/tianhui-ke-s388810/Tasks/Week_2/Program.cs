using Microsoft.EntityFrameworkCore;
using TheaterAdmin.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// SQLite is used here as a stand-in for SQL Express (SQL Express is Windows-only and
// can't run on this machine) — see README.md for the migration path back to a real
// SQL Server/Express instance once available.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=theateradmin.db"));

var app = builder.Build();

// Apply any pending Code First migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");

app.Run();
