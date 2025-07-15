using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Core.Services.TrailAnalysis;

public class RouteAnalyzer: IAnalyzer<List<GpxPoint>, RouteType>
{
    private const double CircularThresholdMeters = 50;  // Distance to consider start/end the same
    private const double OutAndBackSimilarityThreshold = 0.8;  // 80% of points should be similar

    public RouteType Analyze(List<GpxPoint> item)
    {
        return DetermineRouteType(item);
    }
    
    public static RouteType DetermineRouteType(List<GpxPoint> points)
    {
        if (points.Count < 2)
        {
            return RouteType.Unknown;
        }

        var first = points.First();
        var last = points.Last();

        var firstGpxPoint = new GpxPoint(first.Latitude, first.Longitude, first.Elevation);
        var lastGpxPoint = new GpxPoint(last.Latitude, last.Longitude, last.Elevation);
        
        
        // Check if it's a circular route
        if (firstGpxPoint.IsNearby(lastGpxPoint, CircularThresholdMeters))
        {
            return RouteType.Circular;
        }

        // Check if it's an out-and-back route
        return IsOutAndBack(points) 
            ? RouteType.OutAndBack 
            : RouteType.PointToPoint; // If neither circular nor out-and-back, it's point-to-point
    }

    private static bool IsOutAndBack(List<GpxPoint> points)
    {
        if (points.Count < 4) // Need at least 4 points to make a meaningful comparison
            return false;

        var midPoint = points.Count / 2;
        var firstHalf = points.Take(midPoint).ToList();
        var secondHalf = points.Skip(midPoint).Reverse().ToList();

        var matchingPoints = 0;
        var comparisonPoints = Math.Min(firstHalf.Count, secondHalf.Count);

        for (var i = 0; i < comparisonPoints; i++)
        {
            var point1 = new GpxPoint(firstHalf[i].Latitude, firstHalf[i].Longitude);
            var point2 = new GpxPoint(secondHalf[i].Latitude, secondHalf[i].Longitude);

            if (point1.IsNearby(point2, 100)) // Using 100m threshold for path similarity
            {
                matchingPoints++;
            }
        }

        // Calculate what percentage of points match
        var similarity = (double)matchingPoints / comparisonPoints;
        return similarity >= OutAndBackSimilarityThreshold;
    }
}