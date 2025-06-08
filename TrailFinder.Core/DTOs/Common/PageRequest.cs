// TrailFinder.Core/DTOs/Common/PageRequest.cs
namespace TrailFinder.Core.DTOs.Common;

public record PageRequest(
    int PageNumber = 1,
    int PageSize = 10,
    string? SortBy = null,
    bool Descending = false
);