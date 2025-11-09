using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Test_Assigment_for_Codebridge.DataBase;
using Test_Assigment_for_Codebridge.Models;

namespace Tests.DataBase;

public static class TestDbContextFactory
{
    public static AppDbContext CreateInMemory()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbContext = new AppDbContext(options);
        dbContext.Database.EnsureCreated();

        // Seed data
        dbContext.Dogs.AddRange(
            new Dog { Name = "Neo", Color = "red & amber", TailLength = 22, Weight = 32 },
            new Dog { Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 });
        dbContext.SaveChanges();

        return dbContext;
    }
}
