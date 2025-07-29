// TrailFinder.Core.Services.TrailAnalysis\DifficultyAnalyzers\TrailRouteDifficultyAnalyzer.cs (New file)

using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Core.Services.TrailAnalysis.DifficultyAnalysis;

public class TrailRouteDifficultyAnalyzer : IAnalyzer<DifficultyAnalysisInput, DifficultyLevel>
{
    // Define thresholds and scoring weights *specific to trail routes*
    private const double TrailShortDistance = 5000;     // 5km
    private const double TrailModerateDistance = 15000; // 15km
    private const double TrailLongDistance = 30000;     // 30km

    private const double TrailLowElevationGain = 200;    
    private const double TrailModerateElevationGain = 700;
    private const double TrailHighElevationGain = 1500;

    public DifficultyLevel Analyze(DifficultyAnalysisInput item)
    {
        var score = 0;
        
        // Adjust scoring based on trail route characteristics
        score += CalculateTrailDistanceScore(item.TotalDistance);
        score += CalculateTrailElevationScore(item.ElevationGain);
        score += CalculateTrailTerrainScore(item.TerrainType);
        score += CalculateTrailRouteTypeScore(item.RouteType);

        return ConvertScoreToDifficulty(score);
    }

    private static int CalculateTrailDistanceScore(double distance)
    {
        return distance switch
        {
            <= TrailShortDistance => 3, // Shorter trails might still be easy-ish
            <= TrailModerateDistance => 15,
            <= TrailLongDistance => 25,
            _ => 30
        };
    }

    
    private static int CalculateTrailElevationScore(double elevationGain)
    {
        return elevationGain switch
        {
            <= TrailLowElevationGain => 5,
            <= TrailModerateElevationGain => 15,
            <= TrailHighElevationGain => 23,
            _ => 30
        };
    }
    
    private static int CalculateTrailTerrainScore(TerrainType terrainType)
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
    
    private static int CalculateTrailRouteTypeScore(RouteType routeType)
    {
        return routeType switch
        {
            RouteType.Circular => 8,      // Easier as you don't need to arrange transport
            RouteType.OutAndBack => 10,   // Moderate as you can turn back if needed
            RouteType.PointToPoint => 15, // Harder as you need to complete the whole route
            _ => 10
        };
    }
    
    private DifficultyLevel ConvertScoreToDifficulty(int score)
    {
        // Define score ranges specific to trail routes
        return score switch
        {
            < 20 => DifficultyLevel.Easy,
            < 45 => DifficultyLevel.Moderate,
            < 70 => DifficultyLevel.Hard,
            _ => DifficultyLevel.Extreme
        };
    }
}