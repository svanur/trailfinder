using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Gpx.Responses;

public record GpxInfoDto(
    double DistanceMeters,
    double ElevationGainMeters,
    DifficultyLevel DifficultyLevel,
    RouteType RouteType,
    TerrainType TerrainType,
    GpxPoint StartPoint,
    GpxPoint EndPoint,
    LineString RouteGeom
);

