// TrailFinder.Core\Entities\Trail.cs
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
        double distanceMeters,
        double elevationGainMeters,
        DifficultyLevel difficultyLevel,
        RouteType routeType,
        TerrainType terrainType,
        SurfaceType surfaceType,
        LineString routeGeom,
        Guid createdBy,
        DateTime createdAt,
        Guid updatedBy,
        DateTime updatedAt
    )
    {
        Id = id;
        Name = name;
        Slug = GenerateSlug(name);
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
    //private static readonly GeometryFactory GeometryFactory = 
    //    new GeometryFactory(new PrecisionModel(), 4326);

    public string Name { get; set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Description { get; set; } = null!;

    public double DistanceMeters { get; set; }

    public double ElevationGainMeters { get; set; }

    public DifficultyLevel DifficultyLevel { get; set; }
    public RouteType RouteType { get; set; }
    public TerrainType TerrainType { get; set; }
    public SurfaceType SurfaceType { get; set; }

    public LineString? RouteGeom { get; set; }

    // For EF Core to understand the relationships and use Include,
    // one needs navigation properties in the entity classes
    public ICollection<TrailLocation> TrailLocations { get; private set; } = new List<TrailLocation>();
}