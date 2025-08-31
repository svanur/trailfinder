using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Gpx.Requests;

public record UpdateGpxInfoDto(
    double? DistanceMeters,
    double? ElevationGainMeters,
    double? ElevationLossMeters,
    DifficultyLevel? DifficultyLevel,
    RouteType? RouteType,
    TerrainType? TerrainType,
 
    LineString? RouteGeom
);

