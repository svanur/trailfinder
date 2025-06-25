using MediatR;
using TrailFinder.Core.DTOs.Gpx.Responses;

namespace TrailFinder.Application.Features.Trails.Queries.GetTrailGpxInfo;

public record GetTrailGpxInfoQuery(Guid TrailId) : IRequest<GpxInfoDto>;