using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Responses;

public class TrailDto
{
    public TrailDto()
    {
    }

    public TrailDto(
        Guid id,
        string name,
        string slug,
        string description,
        double distance,
        double elevationGain,
        DifficultyLevel? difficultyLevel,
        double? startPointLatitude,
        double? startPointLongitude,
        double? endPointLatitude,
        double? endPointLongitude,
        LineString? routeGeom,
        string? webUrl,
        bool hasGpx,
        DateTime createdAt,
        DateTime updatedAt,
        Guid userId
    )
    {
        Id = id;
        Name = name;
        Slug = slug;
        Description = description;
        Distance = distance;
        ElevationGain = elevationGain;
        DifficultyLevel = difficultyLevel;
        /*
        StartPointLatitude = startPointLatitude;
        StartPointLongitude = startPointLongitude;
        EndPointLatitude = endPointLatitude;
        EndPointLongitude = endPointLongitude;
        */
        RouteGeom = routeGeom;
        WebUrl = webUrl;
        HasGpx = hasGpx;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double Distance { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double ElevationGain { get; set; }

    public DifficultyLevel? DifficultyLevel { get; set; }
    public LineString? RouteGeom { get; set; }

    public string? WebUrl { get; set; }
    public bool HasGpx { get; set; }

    public IEnumerable<TrailLocationDto> TrailLocations { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }
}