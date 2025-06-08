// TrailFinder.Core/Extensions/TrailExtensions.cs
using TrailFinder.Core.DTOs;
using TrailFinder.Core.DTOs.Trails;
using TrailFinder.Core.Entities;

namespace TrailFinder.Core.Extensions;

public static class TrailExtensions
{
    public static TrailDto ToDto(this Trail trail)
    {
        var startCoordinates = trail.GetStartCoordinates();
        
        return new TrailDto(
            Id: trail.Id,
            Name: trail.Name,
            Slug: trail.Slug,
            Description: trail.Description,
            DistanceMeters: trail.DistanceMeters,
            ElevationGainMeters: trail.ElevationGainMeters,
            DifficultyLevel: trail.DifficultyLevel,
            StartPointLatitude: startCoordinates.Latitude,
            StartPointLongitude: startCoordinates.Longitude,
            //RouteCoordinates: trail.GetRouteCoordinates().ToList(),
            WebUrl: trail.WebUrl,
            GpxFilePath: trail.GpxFilePath,
            CreatedAt: trail.CreatedAt,
            UpdatedAt: trail.UpdatedAt,
            UserId: trail.UserId
        );
    }
}