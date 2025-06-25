using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record UpdateTrailDto(
    string Name,
    string Description,
    decimal DistanceMeters,
    decimal ElevationGainMeters,
    DifficultyLevel DifficultyLevel,
    double StartPoint,
    string? WebUrl,
    bool HasGpx
);