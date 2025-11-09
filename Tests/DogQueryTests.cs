using Microsoft.EntityFrameworkCore;
using Test_Assigment_for_Codebridge.DataBase;
using Test_Assigment_for_Codebridge.Models;
using Test_Assigment_for_Codebridge.Models.SortingAndPagination.DTOs;
using Test_Assigment_for_Codebridge.Services;
using Tests.DataBase;

namespace Tests;

public class DogQueryTests
{
    private static async Task<AppDbContext> CreateCleanDbAsync()
    {
        var db = TestDbContextFactory.CreateInMemory();

        // Remove seeded data to make predictable test dataset
        db.Dogs.RemoveRange(db.Dogs);
        await db.SaveChangesAsync();

        return db;
    }

    [Fact]
    public async Task SortByName_AscendingAndDescending()
    {
        // Arrange
        await using var db = await CreateCleanDbAsync();
        var service = new DogService(db);

        db.Dogs.AddRange(
            new Dog { Name = "Charlie", Color = "brown", TailLength = 5, Weight = 10 },
            new Dog { Name = "Alpha", Color = "black", TailLength = 3, Weight = 8 },
            new Dog { Name = "Bravo", Color = "white", TailLength = 4, Weight = 9 }
        );
        await db.SaveChangesAsync();

        // Act - Ascending
        var ascQuery = new DogQueryDTO
        {
            Page = 1,
            PageSize = 10,
            SortBy = "name",
            OrderBy = "Ascending"
        };
        var ascResult = await service.GetDogsByQueryAsync(ascQuery);

        // Assert - Ascending
        var ascNames = ascResult.Items.Select(d => d.Name).ToList();
        Assert.Equal(new[] { "Alpha", "Bravo", "Charlie" }, ascNames);

        // Act - Descending
        var descQuery = new DogQueryDTO
        {
            Page = 1,
            PageSize = 10,
            SortBy = "name",
            OrderBy = "Descending"
        };
        var descResult = await service.GetDogsByQueryAsync(descQuery);

        // Assert - Descending
        var descNames = descResult.Items.Select(d => d.Name).ToList();
        Assert.Equal(new[] { "Charlie", "Bravo", "Alpha" }, descNames);
    }

    [Fact]
    public async Task SortByColor_Ascending()
    {
        // Arrange
        await using var db = await CreateCleanDbAsync();
        var service = new DogService(db);

        db.Dogs.AddRange(
            new Dog { Name = "D1", Color = "blue", TailLength = 10, Weight = 20 },
            new Dog { Name = "D2", Color = "amber", TailLength = 5, Weight = 5 },
            new Dog { Name = "D3", Color = "cyan", TailLength = 15, Weight = 30 }
        );
        await db.SaveChangesAsync();

        var colorQuery = new DogQueryDTO
        {
            Page = 1,
            PageSize = 10,
            SortBy = "color",
            OrderBy = "Ascending"
        };

        // Act
        var colorResult = await service.GetDogsByQueryAsync(colorQuery);
        var colorOrder = colorResult.Items.Select(d => d.Color).ToList();

        // Assert
        Assert.Equal(new[] { "amber", "blue", "cyan" }, colorOrder);
    }

    [Fact]
    public async Task SortByTailLength_Ascending()
    {
        // Arrange
        await using var db = await CreateCleanDbAsync();
        var service = new DogService(db);

        db.Dogs.AddRange(
            new Dog { Name = "D1", Color = "blue", TailLength = 10, Weight = 20 },
            new Dog { Name = "D2", Color = "amber", TailLength = 5, Weight = 5 },
            new Dog { Name = "D3", Color = "cyan", TailLength = 15, Weight = 30 }
        );
        await db.SaveChangesAsync();

        var tailQuery = new DogQueryDTO
        {
            Page = 1,
            PageSize = 10,
            SortBy = "tail_length",
            OrderBy = "Ascending"
        };

        // Act
        var tailResult = await service.GetDogsByQueryAsync(tailQuery);
        var tailOrder = tailResult.Items.Select(d => d.TailLength).ToList();

        // Assert
        Assert.Equal(new[] { 5, 10, 15 }, tailOrder);
    }

    [Fact]
    public async Task SortByWeight_Descending()
    {
        // Arrange
        await using var db = await CreateCleanDbAsync();
        var service = new DogService(db);

        db.Dogs.AddRange(
            new Dog { Name = "D1", Color = "blue", TailLength = 10, Weight = 20 },
            new Dog { Name = "D2", Color = "amber", TailLength = 5, Weight = 5 },
            new Dog { Name = "D3", Color = "cyan", TailLength = 15, Weight = 30 }
        );
        await db.SaveChangesAsync();

        var weightQuery = new DogQueryDTO
        {
            Page = 1,
            PageSize = 10,
            SortBy = "weight",
            OrderBy = "Descending"
        };

        // Act
        var weightResult = await service.GetDogsByQueryAsync(weightQuery);
        var weightOrder = weightResult.Items.Select(d => d.Weight).ToList();

        // Assert
        Assert.Equal(new[] { 30, 20, 5 }, weightOrder);
    }

    [Fact]
    public async Task Pagination_Works_As_Expected()
    {
        // Arrange
        await using var db = await CreateCleanDbAsync();
        var service = new DogService(db);

        // Add 5 dogs
        for (int i = 1; i <= 5; i++)
        {
            db.Dogs.Add(new Dog { Name = $"Dog{i}", Color = "c", TailLength = i, Weight = i * 2 });
        }
        await db.SaveChangesAsync();

        // PageSize = 2, Page = 2 => should return items 3 and 4 (ordered by id ascending)
        var pageQuery = new DogQueryDTO
        {
            Page = 2,
            PageSize = 2,
            SortBy = "id",
            OrderBy = "Ascending"
        };

        // Act
        var pageResult = await service.GetDogsByQueryAsync(pageQuery);

        // Assert
        Assert.Equal(2, pageResult.Items.Count());        
        var returnedIds = pageResult.Items.Select(d => d.Id).ToList();
        var allIdsOrdered = await db.Dogs.OrderBy(d => d.Id).Select(d => d.Id).ToListAsync();
        var expectedPageIds = allIdsOrdered.Skip((pageQuery.Page - 1) * pageQuery.PageSize).Take(pageQuery.PageSize).ToList();
        Assert.Equal(expectedPageIds, returnedIds);
    }
    
    [Fact]
    public async Task ErrorQuery_InvalidPage_Throws()
    {
        // Arrange
        await using var db = await CreateCleanDbAsync();
        var service = new DogService(db);

        db.Dogs.AddRange(
            new Dog { Name = "A", Color = "c", TailLength = 1, Weight = 1 },
            new Dog { Name = "B", Color = "c", TailLength = 2, Weight = 2 }
        );
        await db.SaveChangesAsync();

        var invalidQuery = new DogQueryDTO
        {
            Page = 0,
            PageSize = -10,
            SortBy = "age",
            OrderBy = "none"
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetDogsByQueryAsync(invalidQuery));
    }
}