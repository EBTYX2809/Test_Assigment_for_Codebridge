using Test_Assigment_for_Codebridge.Models.SortingAndPagination.Enums;

namespace Test_Assigment_for_Codebridge.Models.SortingAndPagination.DTOs;

public class DogQueryDTO
{    
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;    
    public SortingAttribute SortBy { get; set; }
    public SortingOrder OrderBy { get; set; }
}
