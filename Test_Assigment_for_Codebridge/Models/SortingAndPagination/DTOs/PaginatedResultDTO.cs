namespace Test_Assigment_for_Codebridge.Models.SortingAndPagination.DTOs;

public class PaginatedResultDTO<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
