using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record UpdateTrailDto(
    string? Name,
    string? Description,
    double? DistanceMeters,
    double? ElevationGainMeters,
    double? ElevationLossMeters,
    DifficultyLevel? DifficultyLevel,
    RouteType? RouteType,
    TerrainType? TerrainType,
    SurfaceType? SurfaceType,
    bool IsActive,
//    LineString? RouteGeom,
    Guid UpdatedBy
);