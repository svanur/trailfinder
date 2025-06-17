using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails;

public class TrailDto
{
    // Properties matching the Trail entity
    public string Id { get; set; } = string.Empty; //TODO: Hmm, shouldn't this be a Guid?
    
    public string ParentId { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string Slug { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double DistanceMeters { get; set; }
    
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double ElevationGainMeters { get; set; }
    
    public DifficultyLevel? DifficultyLevel { get; set; }

    public LineString? RouteGeom { get; set; }
    
    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? StartPointLatitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? StartPointLongitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? EndPointLatitude { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? EndPointLongitude { get; set; }
    
    public string? WebUrl { get; set; }
    
    public bool HasGpx { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public string UserId { get; set; } = string.Empty;

    public TrailDto()
    {
    }

    public TrailDto
        (
            Guid newGuid, 
            Guid? parentId, 
            string name, 
            string slug, 
            string description, 
            double distanceMeters, 
            double elevationGainMeters, 
            DifficultyLevel difficultyLevel, 
            double startPointLatitude, 
            double startPointLongitude, 
            LineString? routeGeometry, 
            string? webUrl, 
            bool hasGpx, 
            DateTime createdAt, 
            DateTime updatedAt, 
            Guid guid
        )
    {
        DistanceMeters = distanceMeters;
        ElevationGainMeters = elevationGainMeters;
        DifficultyLevel = difficultyLevel;
        //RouteGeom = routeGeom;
        //StartPoint = startPoint;
        StartPointLatitude = startPointLatitude;
        StartPointLongitude = startPointLongitude;
        WebUrl = webUrl;
        HasGpx = hasGpx;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}