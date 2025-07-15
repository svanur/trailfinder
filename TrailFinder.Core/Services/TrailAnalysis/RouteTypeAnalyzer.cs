using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;

namespace TrailFinder.Infrastructure.Analyzers;

public class RouteAnalyzer
{
    private const double CircularThresholdMeters = 50;  // Distance to consider start/end the same
    private const double OutAndBackSimilarityThreshold = 0.8;  // 80% of points should be similar

    public static RouteType DetermineRouteType(List<GpxPoint> points)
    {
        if (points == null || points.Count < 2)
            return RouteType.Unknown;

        var startPoint = new GpxPoint(points.First().Latitude, points.First().Longitude);
        var endPoint = new GpxPoint(points.Last().Latitude, points.Last().Longitude);

        // Check if it's a circular route
        if (startPoint.IsNearby(endPoint, CircularThresholdMeters))
            return RouteType.Circular;

        // Check if it's an out-and-back route
        if (IsOutAndBack(points))
            return RouteType.OutAndBack;

        // If neither circular nor out-and-back, it's point-to-point
        return RouteType.PointToPoint;
    }

    private static bool IsOutAndBack(List<GpxPoint> points)
    {
        if (points.Count < 4) // Need at least 4 points to make a meaningful comparison
            return false;

        var midPoint = points.Count / 2;
        var firstHalf = points.Take(midPoint).ToList();
        var secondHalf = points.Skip(midPoint).Reverse().ToList();

        int matchingPoints = 0;
        int comparisonPoints = Math.Min(firstHalf.Count, secondHalf.Count);

        for (int i = 0; i < comparisonPoints; i++)
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