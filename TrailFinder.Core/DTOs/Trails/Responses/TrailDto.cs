using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Responses;

public class TrailDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double DistanceMeters { get; set; }
    
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double ElevationGainMeters { get; set; }
    
    public DifficultyLevel? DifficultyLevel { get; set; }
    public RouteType? RouteType { get; set; }
    public TerrainType? TerrainType { get; set; }
    
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? StartPointLatitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? StartPointLongitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? EndPointLatitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? EndPointLongitude { get; set; }
    
    public LineString? RouteGeom { get; set; }
    
    public string? WebUrl { get; set; }
    public bool HasGpx { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserId { get; set; }

    public TrailDto() { }

    public TrailDto(
        Guid id,
        string name,
        string slug,
        string description,
        double distanceMeters,
        double elevationGainMeters,
        DifficultyLevel? difficultyLevel,
        RouteType routeType, 
        TerrainType terrainType,
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
        DistanceMeters = distanceMeters;
        ElevationGainMeters = elevationGainMeters;
        DifficultyLevel = difficultyLevel;
        RouteType = routeType;
        TerrainType = terrainType;
        StartPointLatitude = startPointLatitude;
        StartPointLongitude = startPointLongitude;
        EndPointLatitude = endPointLatitude;
        EndPointLongitude = endPointLongitude;
        RouteGeom = routeGeom;
        WebUrl = webUrl;
        HasGpx = hasGpx;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }
}