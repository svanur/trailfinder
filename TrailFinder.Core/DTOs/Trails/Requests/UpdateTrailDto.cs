using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record UpdateTrailDto(
    string Name,
    string Description,
    decimal Distance,
    decimal ElevationGain,
    DifficultyLevel DifficultyLevel,
    double StartPoint,
    string? WebUrl,
    bool HasGpx
);