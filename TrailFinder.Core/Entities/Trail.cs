using NetTopologySuite.Geometries;
using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.Entities;

public class Trail : BaseEntity
{
    private static readonly GeometryFactory GeometryFactory = 
        new GeometryFactory(new PrecisionModel(), 4326);

    public string Name { get; set; } = null!;
    public string Slug { get; private set; } = null!;

    public string Description { get; set; } = null!;

    public double Distance { get; init; }
    public double ElevationGain { get; init; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public RouteType RouteType { get; set; }
    public TerrainType TerrainType { get; set; }
    public LineString? RouteGeom { get; set; } = null!;
    public string? WebUrl { get; private set; }
    public bool HasGpx { get; set; }

    private Trail() { } // For EF Core

    public Trail(
        Guid id,
        string name,
        string description,
        double distance,
        double elevationGain,
        DifficultyLevel difficultyLevel,
        RouteType routeType,
        TerrainType terrainType,
        LineString? routeGeom,
        Guid? userId
    )
    {
        Id = id;
        Name = name;
        Description = description;
        Distance = distance;
        ElevationGain = elevationGain;
        DifficultyLevel = difficultyLevel;
        RouteType = routeType;
        TerrainType = terrainType;
        RouteGeom = routeGeom;
        UserId = userId;

        Slug = GenerateSlug(name);
 
        // CreatedAt = DateTime.UtcNow;
        // UpdatedAt = DateTime.UtcNow;
    }

    private static string GenerateSlug(string name)
    {
        return name.ToLower()
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace("/", "-");
    }

    public void Update(
        string name,
        string description,
        // double distance,
        // double elevationGain,
        DifficultyLevel difficultyLevel,
        RouteType routeType,
        TerrainType terrainType
        // double startPointLatitude,
        // double startPointLongitude,
        // double endPointLatitude,
        // double endPointLongitude
    )
    {
        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        // Distance = distance;
        // ElevationGain = elevationGain;
        DifficultyLevel = difficultyLevel;
        RouteType = routeType;
        TerrainType = terrainType; 
        UpdatedAt = DateTime.UtcNow;
    }

    
    
    /*
    public void UpdateRouteGeometry(IEnumerable<(double Latitude, double Longitude, double? Elevation)> coordinates)
    {
        var routeCoordinates = coordinates
            .Select(c => new CoordinateZ(c.Longitude, c.Latitude, c.Elevation ?? 0))
            .ToArray();

        RouteGeom = GeometryFactory.CreateLineString(routeCoordinates);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateGpxFile(string gpxFilePath)
    {
        //HasGpx = true;
        UpdatedAt = DateTime.UtcNow;
    }


    

    public (double? Latitude, double? Longitude)? GetEndCoordinates()
    {
        if (EndPoint == null)
        {
            return null;
        }

        return (EndPoint.Y, EndPoint.X);
    }


    public (double? Latitude, double? Longitude)? GetStartCoordinates()
    {
        if (StartPoint == null)
        {
            return null;
        }

        return (StartPoint.Y, StartPoint.X);
    }
    
    public IEnumerable<(double Latitude, double Longitude)> GetRouteCoordinates()
    {
        if (RouteGeom == null)
            return [];

        return RouteGeom.Coordinates
            .Select(c => (Latitude: c.Y, Longitude: c.X));
    }

    public double GetDistanceFromPoint(double latitude, double longitude)
    {
        var point = GeometryFactory.CreatePoint(new CoordinateZ(longitude, latitude, 0));
        return StartPoint?.Distance(point) ?? 0;
    }

    public bool IsPointNearTrail(double latitude, double longitude, double toleranceMeters)
    {
        if (RouteGeom == null)
            return false;

        var point = GeometryFactory.CreatePoint(new CoordinateZ(longitude, latitude, 0));
        return RouteGeom.Distance(point) <= toleranceMeters;
    }
    */
}