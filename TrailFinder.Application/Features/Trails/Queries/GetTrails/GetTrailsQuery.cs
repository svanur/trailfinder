using MediatR;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Responses;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrails;

public record GetTrailsQuery : IRequest<PaginatedResult<TrailDto>>;