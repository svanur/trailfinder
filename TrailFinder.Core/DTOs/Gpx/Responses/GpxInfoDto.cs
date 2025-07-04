using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Gpx.Responses;

public record GpxInfoDto(
    double Distance,
    double ElevationGain,
    DifficultyLevel DifficultyLevel,
    GpxPoint StartPoint,
    GpxPoint EndPoint,
    LineString RouteGeom
);

