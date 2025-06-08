using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails;

public record TrailDto(
    Guid Id,
    string Name,
    string Slug,
    string Description,
    decimal DistanceMeters,
    decimal ElevationGainMeters,
    TrailDifficulty DifficultyLevel,
    double StartPointLatitude,
    double StartPointLongitude,
    string? WebUrl,
    string? GpxFilePath,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string UserId
);