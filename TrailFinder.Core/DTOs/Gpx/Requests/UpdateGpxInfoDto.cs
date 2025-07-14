using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Gpx.Requests;

public record UpdateGpxInfoDto(
    double? DistanceMeters,
    double? ElevationGainMeters,
    DifficultyLevel? DifficultyLevel,
    RouteType? RouteType,
    TerrainType? TerrainType,
    GpxPoint? StartPoint,
    GpxPoint? EndPoint,
    LineString? RouteGeom
);

