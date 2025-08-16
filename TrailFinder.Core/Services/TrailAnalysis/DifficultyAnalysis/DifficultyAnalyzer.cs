// \TrailFinder.Core\Services\TrailAnalysis\DifficultyAnalyzer.cs

using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Core.Services.TrailAnalysis.DifficultyAnalysis;

/// <summary>
/// Provides methods to analyze the difficulty level of a trail
/// based on various parameters such as distance, elevation gain, terrain type, and route type.
/// </summary>
/// <remarks>
/// This difficulty rating system:
/// 1. Uses a 100-point scoring system based on four main factors:
/// - Distance (30 points)
/// - Elevation gain (30 points)
/// - Terrain type (25 points)
/// - Route type (15 points)
/// 
/// 2. Converts the score into five difficulty levels:
/// - Easy (0-19)
/// - Moderate (20-39)
/// - Challenging (40-59)
/// - Difficult (60-79)
/// - Very Difficult (80-100)
/// 
/// 3. Includes a description generator that provides human-readable explanations of the difficulty level
///
/// The scoring system takes into account:
/// - Longer distances are more difficult
///     - More elevation gain is more difficult
/// - More challenging terrain types increase difficulty
///     - Point-to-point routes are generally more challenging than circular or out-and-back routes
/// 
///     You can adjust the thresholds and scoring weights based on your specific needs or local terrain characteristics.
/// 
/// </remarks>
public class DifficultyAnalyzer: IAnalyzer<DifficultyAnalysisInput, DifficultyLevel>
{
    //private const int MaxScore = 100;
    
    // Distance factors (in meters)
    private const double ShortDistance = 10000;     // 10 km
    private const double ModerateDistance = 21000;  // 21 km
    private const double LongDistance = 42000;      // 42 km

    // Elevation factors (in meters)
    private const double LowElevationGain = 200;    
    private const double ModerateElevationGain = 500;
    private const double HighElevationGain = 1000;

    public DifficultyLevel Analyze(DifficultyAnalysisInput item)
    {
        return AnalyzeDifficulty(item.TotalDistance, item.ElevationGain, item.TerrainType, item.RouteType);
    }

    public static DifficultyLevel AnalyzeDifficulty(
        double totalDistance,
        double totalElevationGain,
        TerrainType terrainType,
        RouteType routeType)
    {
        var score = 0;

        // Distance Score (0-30 points)
        score += CalculateDistanceScore(totalDistance);

        // Elevation Score (0-30 points)
        var verticalRatio = totalDistance > 0 ? (totalElevationGain / totalDistance) * 100 : 0; 
        score += CalculateElevationScore(totalElevationGain, verticalRatio);

        // Terrain Score (0-25 points)
        score += CalculateTerrainScore(terrainType);

        // Route Type Score (0-8 points)
        score += CalculateRouteTypeScore(routeType);

        // Convert score to difficulty level
        return score switch
        {
            < 30 => DifficultyLevel.Easy,
            < 50 => DifficultyLevel.Moderate,
            < 70 => DifficultyLevel.Hard,
            _ => DifficultyLevel.Extreme
            /*
            < 60 => DifficultyLevel.Challenging,
            < 80 => DifficultyLevel.Difficult,
            _ => DifficultyLevel.VeryDifficult
            */
            
        };
    }

    public static string GetDifficultyDescription(
        DifficultyLevel difficulty,
        double distanceKm,
        double elevationGain,
        TerrainType terrain)
    {
        return difficulty switch
        {
            DifficultyLevel.Easy => $"An easy {distanceKm:F1}km trail with gentle {terrain.ToString().ToLower()} terrain " +
                                   $"and {elevationGain:F0}m of elevation gain. Suitable for beginners and casual hikers.",
            
            DifficultyLevel.Moderate => $"A moderate {distanceKm:F1}km trail with {terrain.ToString().ToLower()} terrain " +
                                       $"and {elevationGain:F0}m of elevation gain. Good fitness level recommended.",
            
            DifficultyLevel.Hard => $"A challenging {distanceKm:F1}km trail with {terrain.ToString().ToLower()} terrain " +
                                           $"and {elevationGain:F0}m of elevation gain. Requires good fitness and hiking experience.",
            
            DifficultyLevel.Extreme => $"A difficult {distanceKm:F1}km trail with demanding {terrain.ToString().ToLower()} terrain " +
                                         $"and {elevationGain:F0}m of elevation gain. Requires excellent fitness and hiking experience.",
            
            /*
            DifficultyLevel.Challenging => $"A challenging {distanceKm:F1}km trail with {terrain.ToString().ToLower()} terrain " +
                                         $"and {elevationGain:F0}m of elevation gain. Requires good fitness and hiking experience.",
            
            DifficultyLevel.Difficult => $"A difficult {distanceKm:F1}km trail with demanding {terrain.ToString().ToLower()} terrain " +
                                       $"and {elevationGain:F0}m of elevation gain. Requires excellent fitness and hiking experience.",
            
            DifficultyLevel.VeryDifficult => $"A very difficult {distanceKm:F1}km trail with strenuous {terrain.ToString().ToLower()} terrain " +
                                            $"and {elevationGain:F0}m of elevation gain. For experienced hikers with excellent fitness only.",
                                            */
            
            _ => "Difficulty level could not be determined."
        };
    }

    private static int CalculateDistanceScore(double distance)
    {
        return distance switch
        {
            <= ShortDistance => 5,      // 10.000
            <= ModerateDistance => 15,  // 21.000
            <= LongDistance => 23,      // 42.000
            _ => 30
        };
    }

// Inside CalculateElevationScore or a new CalculateVerticalRatioScore
    private static int CalculateElevationScore(double elevationGain, double verticalRatio) // Adjusted signature
    {
        // Combine logic or add a new scoring component based on the vertical ratio
        var score = 0;
        if (verticalRatio > 5.0)
        {
            score += 10; // High steepness
        }

        if (verticalRatio > 10.0)
        {
            score += 15; // Very high steepness
        }
        // ... then apply elevation gain logic on top of this or in a blended way

        return score;
    }

    private static int CalculateTerrainScore(TerrainType terrainType)
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

    private static int CalculateRouteTypeScore(RouteType routeType)
    {
        return routeType switch
        {
            RouteType.Circular => 4,     // Easier as you don't need to arrange transport
            RouteType.OutAndBack => 6,   // Moderate as you can turn back if needed
            RouteType.PointToPoint => 8, // Harder as you need to complete the whole route
            _ => 10
        };
    }
}
