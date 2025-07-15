using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record CreateTrailDto(
    string Name,
    string Slug,
    string Description,
    //decimal Distance,
    //decimal ElevationGain,
    DifficultyLevel DifficultyLevel,
    RouteType routeType,
    TerrainType terrainType,
    GpxPoint startPoint,
    GpxPoint endPoint,
    //double StartPointLatitude,
    //double StartPointLongitude,
    string? WebUrl,
    bool HasGpx,
    Guid UserId
);