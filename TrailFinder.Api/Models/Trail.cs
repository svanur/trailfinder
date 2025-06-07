// Models/Trail.cs
namespace TrailFinder.Api.Models;

public class Trail
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal DistanceMeters { get; set; }
    public decimal ElevationGainMeters { get; set; }
    public string? DifficultyLevel { get; set; }
    public object? RouteGeom { get; set; }
    public object? StartPoint { get; set; }
    public double? StartPointLatitude { get; set; }
    public double? StartPointLongitude { get; set; }
    public string? WebUrl { get; set; }
    public string? GpxFilePath { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UserId { get; set; } = default!;
}