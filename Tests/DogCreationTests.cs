using Microsoft.EntityFrameworkCore;
using Test_Assigment_for_Codebridge.DataBase;
using Test_Assigment_for_Codebridge.Models;
using Test_Assigment_for_Codebridge.Models.Validators;
using Test_Assigment_for_Codebridge.Services;
using Tests.DataBase;

namespace Tests;

public class DogCreationTests
{
    private readonly AppDbContext _dbContext;
    private readonly DogService _dogService;

    public DogCreationTests()
    {
        _dbContext = TestDbContextFactory.CreateInMemory();
        _dogService = new DogService(_dbContext);
    }

    [Fact]
    public async Task CreateDog_AsExpected()
    {
        // Arrange
        var newDog = new Dog
        {
            Name = "Doggy",
            Color = "brown",
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
     
    [Fact]
    public async Task CreateDog_InvalidData_ThrowsException()
    {
        // Arrange
        var invalidDog = new Dog
        {
            Name = "",
            Color = "blue",
            TailLength = 0,
            Weight = 0
        };

        // Act & Assert
        DogValidator validator = new DogValidator();
        var error = validator.Validate(invalidDog);
        Assert.NotNull(error.Errors);
        Assert.False(error.IsValid);        
    }

    [Fact]
    public async Task CreateDog_DuplicateName_ThrowsArgumentException()
    {
        // Arrange
        // TestDbContextFactory seeds the in-memory DB with dogs "Neo" and "Jessy"
        var duplicateDog = new Dog
        {
            Name = "Neo",
            Color = "red",
            TailLength = 10,
            Weight = 20
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await _dogService.CreateDogAsync(duplicateDog);
        });
    }
}
