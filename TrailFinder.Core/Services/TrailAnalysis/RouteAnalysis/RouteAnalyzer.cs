// TrailFinder.Core\Services\TrailAnalysis\RouteAnalysis\RouteAnalyzer.cs
// (Example of how your RouteAnalyzer class might look after implementing IAnalyzer)

using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;

namespace TrailFinder.Core.Services.TrailAnalysis.RouteAnalysis;

public class RouteAnalyzer : IAnalyzer<List<GpxPoint>, RouteType>
{
    // Threshold for considering start/end points nearby for circular routes (e.g., 50 meters)
    private const double CircularThresholdMeters = 50; 
    // Threshold for considering an "out-and-back" reversal (e.g., how close does the midpoint need to be to the start)
    private const double OutAndBackThresholdMeters = 100; // This might need tuning

    // The Analyze method must be public and non-static as it implements the interface
    public RouteType Analyze(List<GpxPoint> points)
    {
        if (points.Count < 2)
        {
            return RouteType.Unknown;
        }

        var start = points.First();
        var end = points.Last();

        // 1. Check for Circular
        if (IsCircular(start, end, CircularThresholdMeters))
        {
            return RouteType.Circular;
        }

        // 2. Check for Out-and-Back
        return IsOutAndBack(points, OutAndBackThresholdMeters) 
            ? RouteType.OutAndBack
            : RouteType.PointToPoint; // 3. Default to Point-to-Point
    }

    // Helper method (private static, as it doesn't need instance state)
    private static bool IsCircular(GpxPoint start, GpxPoint end, double threshold)
    {
        // Re-use your GpxPoint.IsNearby method
        return start.IsNearby(end, threshold);
    }

    // Helper method for Out-and-Back (private static)
    private static bool IsOutAndBack(List<GpxPoint> points, double threshold)
    {
        if (points.Count < 4) // Need at least A-B-C-A' or A-B-A
        {
            return false;
        }

        // Simplified placeholder logic for IsOutAndBack:
        // This is where the core "out-and-back" detection logic would go.
        // A common approach:
        // 1. Find the approximate "midpoint" of the track (e.g., by distance or index).
        // 2. Compare the first half of the track (e.g., points[0] to midpoint)
        //    with the second half of the track in reverse order (midpoint to points.Last()).
        // 3. This comparison might involve checking if corresponding points are "nearby"
        //    or if the overall shape/sequence of points has a high correlation.
        // This is significantly more complex than simple `IsNearby`.

        // For the sake of the test, let's assume a basic check:
        // If the path length is roughly twice the distance from start to approx middle,
        // and the end point is NOT near the start.
        
        
        var midPoint = points.Count / 2;
        var firstHalf = points.Take(midPoint).ToList();
        var secondHalf = points.Skip(midPoint).Reverse().ToList();

        var matchingPoints = 0;
        var comparisonPoints = Math.Min(firstHalf.Count, secondHalf.Count);

        for (var i = 0; i < comparisonPoints; i++)
        {
            var point1 = new GpxPoint(firstHalf[i].Latitude, firstHalf[i].Longitude, firstHalf[i].Elevation);
            var point2 = new GpxPoint(secondHalf[i].Latitude, secondHalf[i].Longitude, secondHalf[i].Elevation);

            if (point1.IsNearby(point2, 100)) // Using the 100m threshold for path similarity
            {
                matchingPoints++;
            }
        }

        // Calculate what percentage of points match
        var similarity = (double)matchingPoints / comparisonPoints;
        var result = similarity >= threshold;
        
        // This is a very rough heuristic for testing purposes.
        // You'll need a proper algorithm here.
        var totalDistance = CalculateTotalDistance(points); // Need to calculate total distance
        var firstHalfDistance = CalculateTotalDistance(points.Take(points.Count / 2).ToList()); // distance of first half
        
        // Very basic heuristic: if end is not near start, and the overall shape implies reversal
        // This would involve comparing shape similarity, not just distance.
        // A proper implementation might use dynamic time warping or other sequence comparison algorithms.
        
        // For testing, let's simplify for now: assume it's out-and-back if it's not circular
        // and the general shape (as suggested by your test cases) is reversed.
        
        // More sophisticated (but still simplified) out-and-back check:
        // This assumes the track comes back towards the start, but doesn't close the loop.
        // It's often indicated by a significantly reduced overall displacement from start to end
        // compared to the total distance travelled.
        
        // Placeholder, needs proper implementation in RouteAnalyzer:
        // Example: If the track goes far, and then comes back close to the start point,
        // but not *within* the circular threshold.
        // This implies a U-turn or similar.
        var start = points.First();
        var end = points.Last();
        var startToEndDistance = start.CalculateDistance(end); // Assuming GpxPoint has this helper
        
        return startToEndDistance > CircularThresholdMeters && startToEndDistance < totalDistance * 0.5; // Example heuristic
        // Default for unimplemented complex logic
    }
    
    // You'd need a CalculateTotalDistance here, or pass it in, or make it accessible
    private static double CalculateTotalDistance(List<GpxPoint> points)
    {
        if (points == null || points.Count < 2)
        {
            return 0;
        }

        double totalDistance = 0;
        for (var i = 0; i < points.Count - 1; i++)
        {
            var point1 = points[i];
            var point2 = points[i + 1];
        
            const double radiansPerDegree = Math.PI / 180;
            const double earthRadiusMeters = 6371e3;
        
            // Athuga hvort hnit séu gild
            if (point1.Latitude is < -90 or > 90 || 
                point1.Longitude is < -180 or > 180 ||
                point2.Latitude is < -90 or > 90 || 
                point2.Longitude is < -180 or > 180)
            {
                continue;
            }

            var lat1Rad = point1.Latitude * radiansPerDegree;
            var lon1Rad = point1.Longitude * radiansPerDegree;
            var lat2Rad = point2.Latitude * radiansPerDegree;
            var lon2Rad = point2.Longitude * radiansPerDegree;

            var latDiff = lat2Rad - lat1Rad;
            var lonDiff = lon2Rad - lon1Rad;

            var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(lonDiff / 2) * Math.Sin(lonDiff / 2);
        
            // Athuga hvort 'a' sé gilt fyrir frekari útreikninga
            if (!double.IsNaN(a) && a <= 1)
            {
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                var distance = earthRadiusMeters * c;

                if (!double.IsNaN(distance))
                {
                    totalDistance += distance;
                }
            }
        }

        return Math.Round(totalDistance);
    }
}