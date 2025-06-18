using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Gpx;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrailGpxInfo;

public record UpdateTrailGpxInfoCommand(
    Guid TrailId,
    double DistanceMeters,
    double ElevationGainMeters,
    GpxPoint StartPoint,
    GpxPoint EndPoint,
    LineString RouteGeom

) : IRequest<Unit>;
