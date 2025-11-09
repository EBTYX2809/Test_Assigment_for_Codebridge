using Microsoft.EntityFrameworkCore;
using Test_Assigment_for_Codebridge.DataBase;
using Test_Assigment_for_Codebridge.Models;
using Test_Assigment_for_Codebridge.Services;
using Tests.DataBase;

namespace Tests;

public class DogServiceTests
{
    private readonly AppDbContext _dbContext;
    private readonly DogService _dogService;

    public DogServiceTests()
    {
        _dbContext = TestDbContextFactory.CreateInMemory();
        _dogService = new DogService(_dbContext);
    }

    [Fact]
    public async Task CreateGodTest()
    {
        // Arrange
        var newDog = new Dog
        {
            Name = "Doggy",
            Color = "brredown",
            TailLength = 173,
            Weight = 33
        };
        
        // Act
        await _dogService.CreateDogAsync(newDog);
        var createdDog = await _dbContext.Dogs.FirstOrDefaultAsync(d => d.Id == newDog.Id);
        
        // Assert
        Assert.NotNull(createdDog);
        Assert.Equal(newDog.Name, createdDog.Name);
        Assert.Equal(newDog.Color, createdDog.Color);
        Assert.Equal(newDog.TailLength, createdDog.TailLength);
        Assert.Equal(newDog.Weight, createdDog.Weight);
    }
        
}
