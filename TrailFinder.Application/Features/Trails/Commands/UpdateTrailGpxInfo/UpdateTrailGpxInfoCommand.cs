using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrailGpxInfo;

public record UpdateTrailGpxInfoCommand(
    Guid TrailId,
    double DistanceMeters,
    double ElevationGainMeters,
    //DifficultyLevel DifficultyLevel,
    //RouteType RouteType,
    //TerrainType TerrainType,
    GpxPoint StartPoint,
    GpxPoint EndPoint,
    LineString RouteGeom

) : IRequest<Unit>;
