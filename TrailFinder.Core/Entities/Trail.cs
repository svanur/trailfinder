using NetTopologySuite.Geometries;
using TrailFinder.Core.DTOs;
using TrailFinder.Core.Entities.Common;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.Entities;

public class Trail : BaseEntity
{
    private static readonly GeometryFactory GeometryFactory = 
        new GeometryFactory(new PrecisionModel(), 4326);
    
    public Guid? ParentId { get; private set; }
    public string Name { get; private set; } = default!;
    public string Slug { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public decimal DistanceMeters { get; private set; }
    public decimal ElevationGainMeters { get; private set; }
    public DifficultyLevel DifficultyLevelLevel { get; private set; }
    public Point StartPoint { get; private set; } = default!;
    public LineString? RouteGeometry { get; private set; }
    public string? WebUrl { get; private set; }
    public bool HasGpx { get; private set; }

    private Trail() { } // For EF Core

    public Trail(
        Guid? parentId,
        string name,
        string description,
        decimal distanceMeters,
        decimal elevationGainMeters,
        DifficultyLevel difficultyLevelLevel,
        double startPointLatitude,
        double startPointLongitude,
        Guid userId
    )
    {
        Id = Guid.NewGuid();
        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        DistanceMeters = distanceMeters;
        ElevationGainMeters = elevationGainMeters;
        DifficultyLevelLevel = difficultyLevelLevel;
        StartPoint = GeometryFactory.CreatePoint(new Coordinate(startPointLongitude, startPointLatitude));
        ParentId = parentId;
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
        decimal distanceMeters,
        decimal elevationGainMeters,
        DifficultyLevel difficultyLevelLevel,
        double startPointLatitude,
        double startPointLongitude)
    {
        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        DistanceMeters = distanceMeters;
        ElevationGainMeters = elevationGainMeters;
        DifficultyLevelLevel = difficultyLevelLevel;
        StartPoint = GeometryFactory.CreatePoint(new Coordinate(startPointLongitude, startPointLatitude));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRouteGeometry(IEnumerable<(double Latitude, double Longitude)> coordinates)
    {
        var routeCoordinates = coordinates
            .Select(c => new Coordinate(c.Longitude, c.Latitude))
            .ToArray();

        RouteGeometry = GeometryFactory.CreateLineString(routeCoordinates);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateGpxFile(string gpxFilePath)
    {
        //HasGpx = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public (double Latitude, double Longitude) GetStartCoordinates()
    {
        return (StartPoint.Y, StartPoint.X); // Y is the latitude, X is the longitude in spatial types
    }

    public IEnumerable<(double Latitude, double Longitude)> GetRouteCoordinates()
    {
        if (RouteGeometry == null)
            return [];

        return RouteGeometry.Coordinates
            .Select(c => (Latitude: c.Y, Longitude: c.X));
    }

    public double GetDistanceFromPoint(double latitude, double longitude)
    {
        var point = GeometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        return StartPoint.Distance(point);
    }

    public bool IsPointNearTrail(double latitude, double longitude, double toleranceMeters)
    {
        if (RouteGeometry == null)
            return false;

        var point = GeometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        return RouteGeometry.Distance(point) <= toleranceMeters;
    }
}