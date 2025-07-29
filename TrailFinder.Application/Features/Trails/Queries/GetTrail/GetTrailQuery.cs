using MediatR;
using TrailFinder.Core.DTOs.Trails.Responses;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public record GetTrailQuery(Guid Id) : IRequest<TrailDetailDto>;