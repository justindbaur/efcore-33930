
using EfCore33930;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

var migrateSecondDatabase = false;

var connection = new SqliteConnection("DataSource=:memory:");

connection.StateChange += (sender, args) =>
{
    Console.WriteLine($"State Change: Original = {args.OriginalState} Current = {args.CurrentState}");
};

connection.Open();

var services1 = new ServiceCollection()
    .AddDbContext<DatabaseContext>(builder =>
    {
        builder.UseSqlite(connection);
        builder.UseLoggerFactory(NullLoggerFactory.Instance);
    })
    .BuildServiceProvider();

var services2 = new ServiceCollection()
    .AddDbContext<DatabaseContext>(builder =>
    {
        builder.UseSqlite(connection);
        builder.UseLoggerFactory(NullLoggerFactory.Instance);
    })
    .BuildServiceProvider();

var db1 = services1.GetRequiredService<DatabaseContext>();

db1.Database.EnsureDeleted();
db1.Database.EnsureCreated();

db1.Users.Add(new User(Guid.Empty, "Test User"));
db1.SaveChanges();

var db2 = services2.GetRequiredService<DatabaseContext>();

if (migrateSecondDatabase)
{
    db2.Database.EnsureDeleted();
    db2.Database.EnsureCreated();
}
var numberOfUsers = db2.Users.Count();

Console.WriteLine($"{numberOfUsers} user(s)");

connection.Close();
connection.Dispose();
