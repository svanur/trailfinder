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
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public double DistanceMeters { get; set; }
    public double ElevationGainMeters { get; set; }
    public DifficultyLevel? DifficultyLevel { get; private set; }
    public Point? StartPoint { get; set; } = null!;
    public Point? EndPoint { get; set; } = null!;
    public LineString? RouteGeom { get; set; }
    public string? WebUrl { get; private set; }
    public bool HasGpx { get; set; }

    private Trail() { } // For EF Core

    public Trail(
        Guid parentId,
        string name,
        string description,
        double distanceMeters,
        double elevationGainMeters,
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
        DistanceMeters = distanceMeters;
        ElevationGainMeters = elevationGainMeters;
        DifficultyLevel = difficultyLevel;
        StartPoint = GeometryFactory.CreatePoint(new Coordinate(startPointLongitude, startPointLatitude));
        EndPoint = GeometryFactory.CreatePoint(new Coordinate(endPointLongitude, endPointLatitude));
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
        double distanceMeters,
        double elevationGainMeters,
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
        DistanceMeters = distanceMeters;
        ElevationGainMeters = elevationGainMeters;
        DifficultyLevel = difficultyLevel;
        StartPoint = GeometryFactory.CreatePoint(new Coordinate(startPointLongitude, startPointLatitude));
        EndPoint = GeometryFactory.CreatePoint(new Coordinate(endPointLongitude, endPointLatitude));
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRouteGeometry(IEnumerable<(double Latitude, double Longitude)> coordinates)
    {
        var routeCoordinates = coordinates
            .Select(c => new Coordinate(c.Longitude, c.Latitude))
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
        var point = GeometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        return StartPoint.Distance(point);
    }

    public bool IsPointNearTrail(double latitude, double longitude, double toleranceMeters)
    {
        if (RouteGeom == null)
            return false;

        var point = GeometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        return RouteGeom.Distance(point) <= toleranceMeters;
    }
}