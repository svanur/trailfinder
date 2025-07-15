using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Gpx;

public record TrailGpxInfoDto(
    double DistanceMeters,
    double ElevationGainMeters,
    GpxPoint StartPoint,
    GpxPoint EndPoint,
    RouteType RouteType,
    TerrainType TerrainType,
    DifficultyLevel Difficulty,
    LineString RouteGeom
);

