using MediatR;
using TrailFinder.Core.DTOs.Trails;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrail;

public record GetTrailQuery(Guid Id) : IRequest<TrailDto>;