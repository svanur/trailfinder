using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs;
using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.Entities;

public class Trail : BaseEntity
{
    private static readonly GeometryFactory GeometryFactory = 
        new GeometryFactory(new PrecisionModel(), 4326);
    
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DifficultyLevel? DifficultyLevel { get; set; }
    public RouteType? RouteType { get; set; }
    public TerrainType? TerrainType { get; set; }
    public double Distance { get; set; }
    
    public double ElevationGain { get; set; }

    
    public Point? StartPoint { get; set; } = null!;
    public Point? EndPoint { get; set; } = null!;
    public LineString? RouteGeom { get; set; }
    public string? WebUrl { get; private set; }
    public bool HasGpx { get; set; }

    private Trail() { } // For EF Core

    public Trail(
        string name,
        string description,
        double distance,
        double elevationGain,
        DifficultyLevel difficultyLevel,
        double startPointLatitude,
        double startPointLongitude,
        double endPointLatitude,
        double endPointLongitude,
        Guid userId
    )
    {
        Id = Guid.NewGuid();
        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        Distance = distance;
        ElevationGain = elevationGain;
        DifficultyLevel = difficultyLevel;
        StartPoint = GeometryFactory.CreatePoint(new CoordinateZ(startPointLongitude, startPointLatitude, 0));
        EndPoint = GeometryFactory.CreatePoint(new CoordinateZ(endPointLongitude, endPointLatitude, 0));
        
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
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
        double distance,
        double elevationGain,
        DifficultyLevel difficultyLevel,
        double startPointLatitude,
        double startPointLongitude,
        double endPointLatitude,
        double endPointLongitude
    )
    {
        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        Distance = distance;
        ElevationGain = elevationGain;
        DifficultyLevel = difficultyLevel;
        //StartPoint = GeometryFactory.CreatePoint(new CoordinateZ(startPointLongitude, startPointLatitude, 0)); // Add elevation as Z
        //EndPoint = GeometryFactory.CreatePoint(new CoordinateZ(endPointLongitude, endPointLatitude, 0));
        UpdatedAt = DateTime.UtcNow;
    }

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

    public (double? Latitude, double? Longitude)? GetStartCoordinates()
    {
        if (StartPoint == null)
        {
            return null;
        }

        return (StartPoint.Y, StartPoint.X);
    }

    public (double? Latitude, double? Longitude)? GetEndCoordinates()
    {
        if (EndPoint == null)
        {
            return null;
        }

        return (EndPoint.Y, EndPoint.X);
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
}