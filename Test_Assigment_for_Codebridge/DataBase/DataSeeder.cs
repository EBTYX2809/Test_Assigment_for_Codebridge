namespace Test_Assigment_for_Codebridge.DataBase;

public static class DataSeeder
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Dogs.AddRangeAsync(
            new Models.Dog { Name = "Neo", Color = "red & amber", TailLength = 22, Weight = 32 },
            new Models.Dog { Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 });

        dbContext.SaveChangesAsync();
    }
}
