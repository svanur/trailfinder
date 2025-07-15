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
    string? WebUrl,
    bool HasGpx,
    Guid UserId
);