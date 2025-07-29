// TrailFinder.Core.Services.TrailAnalysis\DifficultyAnalyzers\PavedRouteDifficultyAnalyzer.cs (New file)

using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Core.Services.TrailAnalysis.DifficultyAnalysis;

public class PavedRouteDifficultyAnalyzer : IAnalyzer<DifficultyAnalysisInput, DifficultyLevel>
{
    // Define thresholds and scoring weights *specific to paved routes*
    private const double PavedShortDistance = 10000;        // 10km
    private const double PavedModerateDistance = 21000;     // Half-marathon
    private const double PavedLongDistance = 42000;         // Marathon

    // Elevation factors (in meters)
    private const double PavedLowElevationGain = 50;        // 50m
    private const double PavedModerateElevationGain = 150;  // 150m
    private const double PavedHighElevationGain = 300;      // 300m

    public DifficultyLevel Analyze(DifficultyAnalysisInput item)
    {
        var score = 0;
        
        // Adjust scoring based on paved route characteristics
        score += CalculatePavedDistanceScore(item.TotalDistance);
        score += CalculatePavedElevationScore(item.ElevationGain);
        score += CalculatePavedTerrainScore(item.TerrainType); // Terrain still matters, but might have lower weight or different thresholds for paved.
        score += CalculatePavedRouteTypeScore(item.RouteType); // Route type might be less impactful for paved?

        return ConvertScoreToDifficulty(score);
    }

    private static int CalculatePavedDistanceScore(double distance)
    {
        return distance switch
        {
            <= PavedShortDistance => 5,
            <= PavedModerateDistance => 15,
            <= PavedLongDistance => 25,
            _ => 30 // A marathon-plus is definitely challenging on a paved surface!
        };
    }
    
    private static int CalculatePavedElevationScore(double elevationGain)
    {
        return elevationGain switch
        {
            <= PavedLowElevationGain => 5,
            <= PavedModerateElevationGain => 15,
            <= PavedHighElevationGain => 23,
            _ => 30
        };
    }
    
    private static int CalculatePavedTerrainScore(TerrainType terrainType)
    {
        return terrainType switch
        {
            TerrainType.Flat => 5,
            TerrainType.Rolling => 12,
            TerrainType.Hilly => 18,
            TerrainType.Mountainous => 25,
            _ => 0
        };
    }
    
    private static int CalculatePavedRouteTypeScore(RouteType routeType)
    {
        return routeType switch
        {
            RouteType.Circular => 8,      // Easier as you don't need to arrange transport
            RouteType.OutAndBack => 10,   // Moderate as you can turn back if needed
            RouteType.PointToPoint => 15, // Harder as you need to complete the whole route
            _ => 10
        };
    }
    
    // Implement similar CalculatePavedElevationScore, CalculatePavedTerrainScore, CalculatePavedRouteTypeScore
    // You might also decide that for Paved routes, TerrainType always contributes a minimal score (e.g., 0 or 1 point)
    // or RouteType might have less differentiation.

    private DifficultyLevel ConvertScoreToDifficulty(int score)
    {
        // Define score ranges specific to paved routes
        return score switch
        {
            < 15 => DifficultyLevel.Easy,
            < 30 => DifficultyLevel.Moderate,
            < 50 => DifficultyLevel.Hard,
            _ => DifficultyLevel.Extreme
        };
    }
}