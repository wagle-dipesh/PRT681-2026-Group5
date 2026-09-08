using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.DAL
{
    public class EfBookStoreContext : DbContext
    {
        public EfBookStoreContext(DbContextOptions<EfBookStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Printer> Printers { get; set; } = null!;
    }
}