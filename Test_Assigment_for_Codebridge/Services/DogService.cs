using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Test_Assigment_for_Codebridge.DataBase;
using Test_Assigment_for_Codebridge.Models;
using Test_Assigment_for_Codebridge.Models.SortingAndPagination.DTOs;

namespace Test_Assigment_for_Codebridge.Services;

public class DogService
{
    private readonly AppDbContext _dbContext;

    public DogService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateDogAsync(Dog dog)
    {
        try
        {
            var existingDog = await GetDogByNameAsync(dog.Name);
            throw new ArgumentException("Dog with this name already exists");
        }
        catch (ArgumentNullException) { } // Dog with this name does not exist, allow to create            

        await _dbContext.Dogs.AddAsync(dog);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Dog> GetDogByIdAsync(int dogId)
    {
        var dog = await _dbContext.Dogs.FirstOrDefaultAsync(d => d.Id == dogId) 
            ?? throw new ArgumentNullException("Dog by this Id not found");

        return dog;
    }

    public async Task<Dog> GetDogByNameAsync(string dogName)
    {
        var dog = await _dbContext.Dogs.FirstOrDefaultAsync(d => d.Name == dogName)
            ?? throw new ArgumentNullException("Dog by this Name not found");

        return dog;
    }

    public async Task<PaginatedResultDTO<Dog>> GetDogsByQueryAsync(DogQueryDTO query)
    {
        // Take dogs for querying
        IQueryable<Dog> dogsQuery = _dbContext.Dogs;

        // Apply sorting
        Expression<Func<Dog, object>> keySelector = query.SortBy.ToLower() switch
        {
            "id" => d => d.Id,
            "name" => d => d.Name,
            "color" => d => d.Color,
            "weight" => d => d.Weight,            
            "tail_length" => d => d.TailLength,
            _ => d => d.Id
        };

        // Apply ordering
        dogsQuery = query.OrderBy.ToLower() == "descending"
            ? dogsQuery.OrderByDescending(keySelector)
            : dogsQuery.OrderBy(keySelector);

        // Apply pagination
        var dogs = await dogsQuery.Skip((query.Page-1) * query.PageSize)
                                  .Take(query.PageSize)
                                  .ToListAsync();

        // Prepare paginated result
        var dogsPage = new PaginatedResultDTO<Dog>
        {
            Items = dogs,
            TotalCount = await _dbContext.Dogs.CountAsync(),
            Page = query.Page,
            PageSize = query.PageSize
        };

        return dogsPage;
    }
}
