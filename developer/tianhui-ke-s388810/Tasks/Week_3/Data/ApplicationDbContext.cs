using Microsoft.EntityFrameworkCore;
using TheaterAdminWebApi.Models;

namespace TheaterAdminWebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Movies)
                .HasForeignKey(m => m.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Tell EF Core to ignore the NotMapped CategoryName property (belt-and-braces —
            // [NotMapped] on the model already does this, but explicit here too)
            modelBuilder.Entity<Movie>().Ignore(m => m.CategoryName);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", Code = "ACT" },
                new Category { Id = 2, Name = "Drama", Code = "DRA" },
                new Category { Id = 3, Name = "Horror", Code = "HOR" }
            );
        }
    }
}
