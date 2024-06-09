using Microsoft.EntityFrameworkCore;

namespace EfCore33930;

public record User(Guid Id, string Name);

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}