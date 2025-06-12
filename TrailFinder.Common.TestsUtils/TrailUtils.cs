using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Entities;
using TrailFinder.Core.Enums;

namespace TrailFinder.Common.TestsUtils;

public static class TrailUtils
{
    public static Trail CreateTrail(
        string name,
        Guid? parentId = null,
        string description = "",
        decimal distanceMeters = 0,
        decimal elevationGainMeters = 0,
        DifficultyLevel difficultyLevelLevel = DifficultyLevel.Easy,
        double startPointLatitude = 0,
        double startPointLongitude = 0
    )
    {
        return new Trail
        (
            parentId,
            name,
            description,
            distanceMeters,
            elevationGainMeters,
            difficultyLevelLevel,
            startPointLatitude,
            startPointLongitude,
            Guid.NewGuid()
        );

        /*
    Id = Guid.NewGuid();
    Name = name;
    Slug = GenerateSlug(name);
    Description = description;
    DistanceMeters = distanceMeters;
    ElevationGainMeters = elevationGainMeters;
    DifficultyLevelLevel = difficultyLevelLevel;
    StartPoint = GeometryFactory.CreatePoint(new Coordinate(startPointLongitude, startPointLatitude));
    ParentId = parentId;
    UserId = userId;
    CreatedAt = DateTime.UtcNow;
    UpdatedAt = DateTime.UtcNow;
    */
    }

    public static TrailDto CreateTrailDto(
        string name,
        Guid? parentId = null,
        string slug = "",
        string description = "",
        double distanceMeters = 0,
        double elevationGainMeters = 0,
        DifficultyLevel difficultyLevelLevel = DifficultyLevel.Moderate,
        double startPointLatitude = 0,
        double startPointLongitude = 0,
        LineString? routeGeometry = null,
        string? webUrl = "",
        bool hasGpx = false
    )
    {
        return new TrailDto(
            Guid.NewGuid(),
            parentId,
            name,
            slug,
            description,
            distanceMeters,
            elevationGainMeters,
            difficultyLevelLevel,
            startPointLatitude,
            startPointLongitude,
            routeGeometry,
            webUrl,
            hasGpx,
            DateTime.Now,
            DateTime.Now,
            Guid.NewGuid()
        );
    }
}