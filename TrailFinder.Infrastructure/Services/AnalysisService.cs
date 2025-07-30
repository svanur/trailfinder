// TrailFinder.Infrastructure\Services\AnalysisService.cs

using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Core.Services.TrailAnalysis;
using TrailFinder.Core.ValueObjects;
namespace TrailFinder.Infrastructure.Services;

public class AnalysisService : IAnalysisService
{
    private const double EarthRadiusMeters = 6371e3;
    private const int ElevationPrecisionDecimals = 0; // Round to whole meters

    /// <summary>
    /// Even after smoothing, there might be very small, legitimate ups and downs
    /// that don't represent a significant climb. This threshold allows you to ignore those,
    /// focusing only on meaningful gains.
    /// Common values are 1m, 2m, or 3m, depending on how "strict" you want to be.
    /// </summary>
    private const double ElevationNoiseThresholdMeters = 1.2; // Ignore changes smaller than 2 meters

    /// <summary>
    /// A larger window will produce a smoother profile but might also flatten out legitimate small hills.
    /// A smaller window retains more detail but less smoothing. 3 to 7 is a common range. 5 is a good start.
    /// </summary>
    private const int ElevationSmoothingWindowSize = 8; // Use a window of 5 points for smoothing

    private readonly IAnalyzer<List<GpxPoint>, RouteType> _routeAnalyzer;
    private readonly IAnalyzer<TerrainAnalysisInput, TerrainType> _terrainAnalyzer;
    
    private readonly DifficultyAnalyzerFactory _difficultyAnalyzerFactory;
    
    

    public AnalysisService(
        IAnalyzer<List<GpxPoint>, RouteType> routeAnalyzer,
        IAnalyzer<TerrainAnalysisInput, TerrainType> terrainAnalyzer,
        DifficultyAnalyzerFactory difficultyAnalyzerFactory)
    {
        _routeAnalyzer = routeAnalyzer;
        _terrainAnalyzer = terrainAnalyzer;
        _difficultyAnalyzerFactory = difficultyAnalyzerFactory;
    }

    public AnalysisResult AnalyzeGpxPointsBySurfaceType(List<GpxPoint> points, SurfaceType surfaceType)
    {
        var totalDistance = CalculateTotalDistance(points);
        var elevationPoints = points.Select(p => p.Elevation);
        var elevationGain = CalculateElevationGain(elevationPoints);
        
        // Calculate Vertical Ratio (as a percentage, e.g., 5% = 5m per 100m horizontal)
        // Be careful with division by zero for very short or zero-distance routes
        var verticalRatio = totalDistance > 0 ? (elevationGain / totalDistance) * 100 : 0; 
        
        var terrainType = _terrainAnalyzer.Analyze(
            new TerrainAnalysisInput
            {
                TotalDistance = totalDistance,
                ElevationGain = elevationGain,
                VerticalRatio = verticalRatio
            }
        );
        
        var routeType = _routeAnalyzer.Analyze(points);
        
        // Now get the specific difficulty analyzer based on surfaceType
        var surfaceTypeDifficultyAnalyzer = _difficultyAnalyzerFactory.GetAnalyzer(surfaceType);

        var difficultyInput = DifficultyAnalysisInput.Builder()
            .WithTotalDistance(totalDistance)
            .WithElevationGain(elevationGain)
            .WithTerrainType(terrainType)
            .WithRouteType(routeType)
            .WithVerticalRatio(verticalRatio)
            .Build();

        // Use the specific analyzer
        var difficultyLevel = surfaceTypeDifficultyAnalyzer.Analyze(difficultyInput); 

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
            totalDistance += point1.CalculateDistance(point2);
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
        
        // 1. Apply smoothing to reduce GPS noise
        var smoothedElevations = SmoothElevations(elevations, ElevationSmoothingWindowSize);

        var totalElevationGain = 0.0;
        
        // Start from the first smoothed elevation
        var currentElevation = smoothedElevations[0]; 

        for (var i = 1; i < smoothedElevations.Count; i++)
        {
            var nextElevation = smoothedElevations[i];
            var elevationDiff = nextElevation - currentElevation;

            // Only count uphill sections and filter out noise below the threshold
            if (elevationDiff > ElevationNoiseThresholdMeters) // Only add if it's an actual gain above noise
            {
                totalElevationGain += elevationDiff;
            }
            
            // Always update the current elevation for the next iteration, regardless of gain/loss
            currentElevation = nextElevation; 
        }

        // Round the final total elevation gain
        return Math.Round(totalElevationGain, ElevationPrecisionDecimals);
    }

    /// <summary>
    /// Applies a simple moving average filter to a list of elevations.
    /// </summary>
    /// <param name="elevations">The raw elevation points.</param>
    /// <param name="windowSize">The number of points to include in the moving average window. Must be odd.</param>
    /// <returns>A new list of smoothed elevations.</returns>
    private static List<double> SmoothElevations(List<double> elevations, int windowSize)
    {
        if (windowSize % 2 == 0) // Window size must be odd for symmetric window
        {
            windowSize++; // Increment to make it odd, or throw ArgumentException
        }

        var smoothed = new List<double>(elevations.Count);
        var halfWindow = windowSize / 2;

        for (var i = 0; i < elevations.Count; i++)
        {
            double sum = 0;
            var count = 0;
            
            // Calculate the start and end indices for the current window
            var windowStart = Math.Max(0, i - halfWindow);
            var windowEnd = Math.Min(elevations.Count - 1, i + halfWindow);

            for (var j = windowStart; j <= windowEnd; j++)
            {
                sum += elevations[j];
                count++;
            }
            smoothed.Add(sum / count);
        }
        
        return smoothed;
    }
}