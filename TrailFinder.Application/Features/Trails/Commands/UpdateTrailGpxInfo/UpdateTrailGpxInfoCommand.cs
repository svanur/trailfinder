using MediatR;
using TrailFinder.Core.DTOs.Gpx;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrailGpxInfo;

public record UpdateTrailGpxInfoCommand(
    Guid TrailId,
    double DistanceMeters,
    double ElevationGainMeters,
    GpxPoint StartPoint,
    GpxPoint EndPoint
) : IRequest<Unit>;
