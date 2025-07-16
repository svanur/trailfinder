using MediatR;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Race.Response;

namespace TrailFinder.Application.Features.Races.Queries.GetRaces;

public record GetRacesQuery : IRequest<PaginatedResult<RaceDto>>
{
    public int PageNumber { get; init; } = 1; // 'init' setter for immutability in records
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; } = false;
}