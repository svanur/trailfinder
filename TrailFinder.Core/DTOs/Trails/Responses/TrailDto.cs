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
    public double Distance { get; set; }
    
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double ElevationGainMeters { get; set; }
    
    public DifficultyLevel? DifficultyLevel { get; set; }
    //public RouteType? RouteType { get; set; }
    //public TerrainType? TerrainType { get; set; }
    
    public LineString? RouteGeom { get; set; }
    
    /*
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? StartPointLatitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? StartPointLongitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? EndPointLatitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? EndPointLongitude { get; set; }
    */
    
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
        double distance,
        double elevationGainMeters,
        DifficultyLevel? difficultyLevel,
        /*
        double? startPointLatitude,
        double? startPointLongitude,
        double? endPointLatitude,
        double? endPointLongitude,
        */
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
        ElevationGainMeters = elevationGainMeters;
        DifficultyLevel = difficultyLevel;
        //RouteType = routeType;
        //TerrainType = terrainType;
        
        RouteGeom = routeGeom;
        
        //StartPoint = startPoint;
        //StartPointLatitude = startPointLatitude;
        //StartPointLongitude = startPointLongitude;
        WebUrl = webUrl;
        HasGpx = hasGpx;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }
}