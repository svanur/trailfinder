using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrail;

public record UpdateTrailCommand(
    Guid TrailId,
    double? Distance,
    double? ElevationGain,
    DifficultyLevel? DifficultyLevel,
    RouteType? RouteType,
    TerrainType? TerrainType,
    GpxPoint? StartPoint,
    GpxPoint? EndPoint,
    LineString? RouteGeom

) : IRequest<Unit>;
