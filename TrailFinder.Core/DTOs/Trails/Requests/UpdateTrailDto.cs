using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record UpdateTrailDto(
    string Name,
    string Description,
    decimal Distance,
    decimal ElevationGainMeters,
    DifficultyLevel DifficultyLevel,
    RouteType RouteType,
    TerrainType TerrainType,
    string? WebUrl,
    bool HasGpx
);