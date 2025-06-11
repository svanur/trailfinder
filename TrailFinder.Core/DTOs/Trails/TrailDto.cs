using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails;

public record TrailDto(
    Guid Id,
    Guid? ParentId,
    string Name,
    string Slug,
    string Description,
    decimal DistanceMeters,
    decimal ElevationGainMeters,
    TrailDifficulty DifficultyLevel,
    double StartPointLatitude,
    double StartPointLongitude,
    string? WebUrl,
    bool HasGpx,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string UserId
);