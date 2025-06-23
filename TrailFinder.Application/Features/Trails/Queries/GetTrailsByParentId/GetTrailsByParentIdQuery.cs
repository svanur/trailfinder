using MediatR;
using TrailFinder.Core.DTOs.Common;
using TrailFinder.Core.DTOs.Trails.Responses;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailsByParentId;

public record GetTrailsByParentIdQuery(Guid ParentId) : IRequest<PaginatedResult<TrailDto>>;
