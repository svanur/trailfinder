using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Gpx.Responses;

public record GpxAnalysisResult(
    double Distance,
    double ElevationGain,
    double VerticalRatio,
    DifficultyLevel DifficultyLevel,
    RouteType RouteType,
    TerrainType TerrainType,
    GpxPoint StartGpxPoint,
    GpxPoint EndGpxPoint,
    LineString RouteGeom
);

