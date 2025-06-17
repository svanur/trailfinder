using MediatR;
using TrailFinder.Core.DTOs.Gpx;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailGpxInfo;

public record GetTrailGpxInfoQuery(Guid TrailId) : IRequest<TrailGpxInfoDto>;