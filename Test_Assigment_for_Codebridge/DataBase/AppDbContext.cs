using Microsoft.EntityFrameworkCore;
using Test_Assigment_for_Codebridge.DataBase.Configurations;
using Test_Assigment_for_Codebridge.Models;

namespace Test_Assigment_for_Codebridge.DataBase;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Dog> Dogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DogConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
