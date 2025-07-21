using System.ComponentModel;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.DTOs.Location.Response;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.DTOs.Trails.Responses;

public class TrailDto : BaseDto
{
    public TrailDto()
    {
    }

    public TrailDto(
        Guid id,
        string name,
        string slug,
        string description,
        double distanceMeters,
        double elevationGainMeters,
        DifficultyLevel difficultyLevel,
        RouteType routeType,
        TerrainType terrainType,
        SurfaceType surfaceType,
        LineString? routeGeom,
        GpxPoint startGpxPoint,
        GpxPoint endGpxPoint,
        Guid createdBy,
        DateTime createdAt,
        Guid? updatedBy,
        DateTime? updatedAt
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
        SurfaceType = surfaceType;
        
        RouteGeom = routeGeom;

        CreatedBy = createdBy;
        CreatedAt = createdAt;
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt;
    }
    
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? DistanceMeters { get; set; }
    public double DistanceKm { get; set; }

    [JsonNumberHandling(JsonNumberHandling.AllowNamedFloatingPointLiterals)]
    public double? ElevationGainMeters { get; set; }

    public DifficultyLevel? DifficultyLevel { get; set; }
    public RouteType? RouteType { get; set; }
    public TerrainType? TerrainType { get; set; }
    public SurfaceType? SurfaceType { get; set; }
    
    public GpxPoint StartGpxPoint { get; set; }
    public GpxPoint EndGpxPoint { get; set; }
    
    public LineString? RouteGeom { get; set; }
    
    public IEnumerable<TrailLocationDto> TrailLocations { get; set; }
    
}