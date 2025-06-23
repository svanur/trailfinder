using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Requests;

public record CreateTrailDto(
    string Name,
    string Description,
    decimal DistanceMeters,
    decimal ElevationGainMeters,
    //DifficultyLevel DifficultyLevel,
    double StartPointLatitude,
    double StartPointLongitude,
    string? WebUrl,
    bool HasGpx,
    Guid? UserId
);