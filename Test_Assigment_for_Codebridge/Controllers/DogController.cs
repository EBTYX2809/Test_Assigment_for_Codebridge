using Microsoft.AspNetCore.Mvc;
using Test_Assigment_for_Codebridge.Models;
using Test_Assigment_for_Codebridge.Models.SortingAndPagination.DTOs;
using Test_Assigment_for_Codebridge.Services;

namespace Test_Assigment_for_Codebridge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DogController : ControllerBase
{
    private readonly DogService _dogService;

    public DogController(DogService dogService)
    {
        _dogService = dogService;
    }

    /// <summary>
    /// Get dog by id.
    /// </summary>
    /// <param name="id">Dog id.</param>
    /// <returns>Return dog object.</returns>
    /// <response code="200">Success.</response>
    /// <response code="404">Dog not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<Dog>> GetDogById(int id)
    {
        var dog = await _dogService.GetDogByIdAsync(id);

        return Ok(dog);
    }

    /// <summary>
    /// Get list of dogs.
    /// </summary>
    /// <param name="dogQueryDTO">Query DTO with
    /// Page = Number of page;
    /// PageSize = Size of page;
    /// SortBy = Sort attribute;
    /// OrderBy = Sort order.</param>
    /// <returns>Returns PaginatedResultDTO with list of filtered gods.</returns>
    /// <response code="200">Success.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedResultDTO<Dog>>> GetDogsByQuery([FromQuery] DogQueryDTO dogQueryDTO)
    {
        var result = await _dogService.GetDogsByQueryAsync(dogQueryDTO);

        return Ok(result);
    }

    /// <summary>
    /// Create dog.
    /// </summary>
    /// <param name="dog">The Dog object to create.</param>
    /// <returns>Returns the ID of the created dog.</returns>
    /// <response code="201">Dog successfully created.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="404">Dog not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    public async Task<ActionResult<int>> CreateDog([FromBody] Dog dog)
    {
        await _dogService.CreateDogAsync(dog);

        return CreatedAtAction(nameof(GetDogById), new { id = dog.Id }, dog.Id);
    }
}
