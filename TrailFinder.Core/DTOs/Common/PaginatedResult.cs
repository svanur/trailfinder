// TrailFinder.Core/DTOs/Common/PaginatedResult.cs
namespace TrailFinder.Core.DTOs.Common;

public record PaginatedResult<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}