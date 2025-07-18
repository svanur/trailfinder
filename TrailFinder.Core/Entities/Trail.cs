using NetTopologySuite.Geometries;
using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.Entities;

public class Trail : BaseEntity
{
    private Trail()
    {
    } // For EF Core

    public Trail(
        Guid id,
        string name,
        string description,
        double distance,
        double elevationGain,
        DifficultyLevel difficultyLevel,
        RouteType routeType,
        TerrainType terrainType,
        SurfaceType surfaceType,
        Guid userId
    )
    {
        Id = id;
        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        Distance = distance;
        ElevationGain = elevationGain;
        DifficultyLevel = difficultyLevel;
        RouteType = routeType;
        TerrainType = terrainType;
        SurfaceType = surfaceType;

        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    //private static readonly GeometryFactory GeometryFactory = 
    //    new GeometryFactory(new PrecisionModel(), 4326);

    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public double Distance { get; set; }

    public double ElevationGain { get; set; }

    public DifficultyLevel DifficultyLevel { get; set; }
    public RouteType RouteType { get; set; }
    public TerrainType TerrainType { get; set; }
    public SurfaceType SurfaceType { get; set; }

    public LineString? RouteGeom { get; set; }
    public string? WebUrl { get; private set; }

    // For EF Core to understand the relationships and use Include,
    // one needs navigation properties in the entity classes
    public ICollection<TrailLocation> TrailLocations { get; private set; } = new List<TrailLocation>();
}