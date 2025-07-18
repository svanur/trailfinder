using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record CreateTrailDto(
    string Name,
    string Slug,
    string Description,
    decimal Distance,
    decimal ElevationGain,
    DifficultyLevel DifficultyLevel,
    RouteType RouteType,
    TerrainType TerrainType,
    SurfaceType SurfaceType,
    LineString? RouteGeom,
    GpxPoint StartGpxPoint,
    GpxPoint EndGpxPoint,
    string? WebUrl,
    Guid CreatedBy
);