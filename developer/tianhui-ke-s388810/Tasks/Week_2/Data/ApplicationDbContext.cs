using Microsoft.EntityFrameworkCore;
using TheaterAdmin.Models;

namespace TheaterAdmin.Data
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

            // A movie must have exactly one category (NOT NULL foreign key, as required by the task)
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Category)
                .WithMany(c => c.Movies)
                .HasForeignKey(m => m.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // prevent deleting a category that still has movies

            // Seed a few starter categories, matching the examples given in the task (Action, Drama, Horror)
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", Code = "ACT" },
                new Category { Id = 2, Name = "Drama", Code = "DRA" },
                new Category { Id = 3, Name = "Horror", Code = "HOR" }
            );
        }
    }
}
