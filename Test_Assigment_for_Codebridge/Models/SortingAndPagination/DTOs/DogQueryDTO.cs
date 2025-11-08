using Test_Assigment_for_Codebridge.Models.SortingAndPagination.Enums;

namespace Test_Assigment_for_Codebridge.Models.SortingAndPagination.DTOs;

public class DogQueryDTO
{    
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "Id";
    public string OrderBy { get; set; } = "Ascending";
    // I decided to not using enums now because setting strings better for HTTP requests
    // public SortingAttribute SortBy { get; set; } 
    // public SortingOrder OrderBy { get; set; }
}
