using NetTopologySuite.Geometries;
using Supabase.Postgrest.Attributes;
using TrailFinder.Core.DTOs;
using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.Entities;

public class Location : BaseEntity
{
    public string Name { get; private set; } = null!;
    public Guid? ParentId { get; private set; }
    public string Slug { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    //public LineString? PointGeom { get; set; }
    
    // This is crucial for EF Core to understand the "many" side
    public ICollection<TrailLocation> TrailLocations { get; } = new List<TrailLocation>();
    public ICollection<RaceLocation> RaceLocations { get; } = new List<RaceLocation>();
    
    //public Location? ParentLocation { get; } = new();
    //public IEnumerable<Location> ChildrenLocations { get; } = new List<Location>();
    
    

    private Location() { } // For EF Core

    public Location(
        string name,
        Guid? parentId,
        string description,
        double latitude,
        double longitude,
        Guid userId
    )
    {
        Id = Guid.NewGuid();
        ParentId = parentId;
        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        Latitude = latitude;
        Longitude = longitude;
        UserId = userId;
        
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

}