// TrailFinder.Application.Features.Trails.Queries.GetTrails/GetTrailsQuery.cs

using MediatR;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Responses;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrails;

// Make sure these properties are part of your GetTrailsQuery record
public record GetTrailsQuery : IRequest<PaginatedResult<TrailDto>>
{
    public int PageNumber { get; init; } = 1; // 'init' setter for immutability in records
    public int PageSize { get; init; } = 10;
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; } = false;
}