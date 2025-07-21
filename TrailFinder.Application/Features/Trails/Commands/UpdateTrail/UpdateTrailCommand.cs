using MediatR;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Application.Features.Trails.Commands.UpdateTrail;

public record UpdateTrailCommand(
    Guid TrailId,
    string? Name,
    string? Description,
    double? Distance,
    double? ElevationGain,
    DifficultyLevel? DifficultyLevel,
    RouteType? RouteType,
    TerrainType? TerrainType,
    SurfaceType? SurfaceType,
    LineString? RouteGeom,
    Guid? UpdatedBy
) : IRequest<Unit>;
