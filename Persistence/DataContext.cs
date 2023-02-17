using Domain;
using Microsoft.EntityFrameworkCore;
namespace Persistence;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options):base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseInMemoryDatabase(databaseName:"BookStore");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Book>()
            .Navigation(b => b.Reviews).AutoInclude();
        modelBuilder.Entity<Book>()
            .Navigation(b => b.Ratings).AutoInclude();
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Rating> Ratings { get; set; }
}