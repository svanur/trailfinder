using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails;

public record UpdateTrailDto(
    string Name,
    string Description,
    decimal DistanceMeters,
    decimal ElevationGainMeters,
    TrailDifficulty DifficultyLevel,
    double StartPointLatitude,
    double StartPointLongitude,
    string? WebUrl
);