using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record CreateTrailDto(
    string Name,
    string Slug,
    string Description,
    double Distance,
    double ElevationGain,
    DifficultyLevel DifficultyLevel,
    RouteType RouteType,
    TerrainType TerrainType,
    SurfaceType SurfaceType,
    LineString? RouteGeom,
    //GpxPoint StartGpxPoint,
    //GpxPoint EndGpxPoint,
    Guid CreatedBy
);