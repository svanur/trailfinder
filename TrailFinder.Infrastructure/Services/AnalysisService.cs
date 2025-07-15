using System.Drawing;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Infrastructure.Services;


public class AnalysisService
{
    private const double EarthRadiusMeters = 6371e3;
    private const int ElevationPrecisionDecimals = 0;
    
    private readonly IAnalyzer<List<GpxPoint>, RouteType> _routeAnalyzer;
    private readonly IAnalyzer<TerrainAnalysisInput, TerrainType> _terrainAnalyzer;
    private readonly IAnalyzer<DifficultyAnalysisInput, DifficultyLevel> _difficultyAnalyzer;

    // Constructor remains the same
    public AnalysisService(
        IAnalyzer<List<GpxPoint>, RouteType> routeAnalyzer,
        IAnalyzer<TerrainAnalysisInput, TerrainType> terrainAnalyzer,
        IAnalyzer<DifficultyAnalysisInput, DifficultyLevel> difficultyAnalyzer)
    {
        _routeAnalyzer = routeAnalyzer;
        _terrainAnalyzer = terrainAnalyzer;
        _difficultyAnalyzer = difficultyAnalyzer;
    }

    public AnalysisResult Analyze(List<GpxPoint> points)
    {
        var totalDistance = CalculateTotalDistance(points);
        var elevationPoints = points.Select(p => p.Elevation);
        var elevationGain = CalculateElevationGain(elevationPoints);
        
        var routeType = _routeAnalyzer.Analyze(points);

        // Using object initializer for TerrainAnalysisInput (simple, no builder really needed for just 2 props)
        var terrainInput = new TerrainAnalysisInput
        {
            TotalDistance = totalDistance,
            ElevationGain = elevationGain
        };
        var terrainType = _terrainAnalyzer.Analyze(terrainInput);

        // Using the Builder pattern for DifficultyAnalysisInput
        var difficultyInput = DifficultyAnalysisInput.Builder()
            .WithTotalDistance(totalDistance)
            .WithElevationGain(elevationGain)
            .WithTerrainType(terrainType)
            .WithRouteType(routeType)
            .Build();

        var difficultyLevel = _difficultyAnalyzer.Analyze(difficultyInput);

        return new AnalysisResult(
            totalDistance,
            elevationGain,
            routeType, 
            terrainType, 
            difficultyLevel,
            points.First(),
            points.Last()
        );
    }
    
    private static double CalculateTotalDistance(List<GpxPoint> points)
    {
        double totalDistance = 0;
        for (var i = 0; i < points.Count - 1; i++)
        {
            var point1 = points[i];
            var point2 = points[i + 1];
            totalDistance += CalculateDistance(point1, point2);
        }
        
        return Math.Round(totalDistance);
    }

    private static double CalculateElevationGain(IEnumerable<double> elevationPoints)
    {
        ArgumentNullException.ThrowIfNull(elevationPoints);

        var elevations = elevationPoints.ToList();
        if (elevations.Count == 0)
        {
            throw new ArgumentException("Elevation points collection cannot be empty", nameof(elevationPoints));
        }

        var totalElevationGain = 0.0;
        var currentElevation = elevations[0];
        for (var i = 1; i < elevations.Count; i++)
        {
            var nextElevation = elevations[i];
            var elevationDifference = CalculateUphillDifference(currentElevation, nextElevation);
            totalElevationGain += elevationDifference;
            currentElevation = nextElevation;
        }
        
        return Math.Round(totalElevationGain, ElevationPrecisionDecimals);
    }

    /*
     private static double CalculateElevationGain(IEnumerable<double> elevationPoints)
    {
        ArgumentNullException.ThrowIfNull(elevationPoints);

        var elevations = elevationPoints.ToList();
        if (elevations.Count == 0)
        {
            throw new ArgumentException("Elevation points collection cannot be empty", nameof(elevationPoints));
        }

        const double noiseThreshold = 2.0; // Meters - ignore elevation changes smaller than this
        const int smoothingWindowSize = 3; // Number of points to use for smoothing

        // Apply smoothing to reduce GPS noise
        var smoothedElevations = SmoothElevations(elevations, smoothingWindowSize);

        var totalElevationGain = 0.0;
        var currentElevation = smoothedElevations[0];

        for (var i = 1; i < smoothedElevations.Count; i++)
        {
            var nextElevation = smoothedElevations[i];
            var elevationDiff = nextElevation - currentElevation;

            // Only count uphill sections and filter out noise
            if (elevationDiff > noiseThreshold)
            {
                totalElevationGain += elevationDiff;
            }

            currentElevation = nextElevation;
        }

        return Math.Round(totalElevationGain); // Round to nearest meter
    }
     * 
     */
    
    private static double CalculateUphillDifference(double currentElevation, double nextElevation)
    {
        var difference = nextElevation - currentElevation;
        return difference > 0 
            ? Math.Round(difference, ElevationPrecisionDecimals) 
            : 0;
    }
    
    private static double CalculateDistance(GpxPoint point1, GpxPoint point2)
    {
        const double radiansPerDegree = Math.PI / 180;
    
        // Check for valid coordinate ranges
        if (!IsValidCoordinate(point1) || !IsValidCoordinate(point2))
        {
            return 0;
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
            
        // Check if 'a' is valid for further calculation
        if (double.IsNaN(a) || a > 1)
        {
            return 0;
        }

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = EarthRadiusMeters * c;

        return double.IsNaN(distance) 
            ? 0 
            : distance;
    }

    private static bool IsValidCoordinate(GpxPoint point)
    {
        return point.Latitude is >= -90 and <= 90 &&
               point.Longitude is >= -180 and <= 180 &&
               !double.IsNaN(point.Latitude) && !double.IsNaN(point.Longitude);
    }
}