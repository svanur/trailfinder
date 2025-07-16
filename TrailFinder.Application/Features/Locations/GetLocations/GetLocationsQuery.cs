using MediatR;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Location.Response;

namespace TrailFinder.Application.Features.Locations.GetLocations;

public record GetLocationsQuery : IRequest<PaginatedResult<LocationDto>>
{
    public int PageNumber { get; init; } = 1; // 'init' setter for immutability in records
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; } = false;
}
