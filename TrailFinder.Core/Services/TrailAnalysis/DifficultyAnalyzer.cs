using TrailFinder.Core.Enums;

namespace TrailFinder.Infrastructure.Analyzers;

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
public class DifficultyAnalyzer
{
    private const int MaxScore = 100;
    
    // Distance factors (in meters)
    private const double ShortDistance = 5000;     // 5km
    private const double ModerateDistance = 10000; // 10km
    private const double LongDistance = 21000;     // 20km

    // Elevation factors (in meters)
    private const double LowElevationGain = 200;    
    private const double ModerateElevationGain = 500;
    private const double HighElevationGain = 1000;

    public static DifficultyLevel AnalyzeDifficulty(
        double distance,
        double elevationGain,
        TerrainType terrainType,
        RouteType routeType)
    {
        var score = 0;

        // Distance Score (0-30 points)
        score += CalculateDistanceScore(distance);

        // Elevation Score (0-30 points)
        score += CalculateElevationScore(elevationGain);

        // Terrain Score (0-25 points)
        score += CalculateTerrainScore(terrainType);

        // Route Type Score (0-15 points)
        score += CalculateRouteTypeScore(routeType);

        // Convert score to difficulty level
        return score switch
        {
            < 20 => DifficultyLevel.Easy,
            < 40 => DifficultyLevel.Moderate,
            < 60 => DifficultyLevel.Hard,
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
            <= ShortDistance => 5,
            <= ModerateDistance => 15,
            <= LongDistance => 23,
            _ => 30
        };
    }

    private static int CalculateElevationScore(double elevationGain)
    {
        return elevationGain switch
        {
            <= LowElevationGain => 5,
            <= ModerateElevationGain => 15,
            <= HighElevationGain => 23,
            _ => 30
        };
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
            RouteType.Circular => 8,      // Easier as you don't need to arrange transport
            RouteType.OutAndBack => 10,   // Moderate as you can turn back if needed
            RouteType.PointToPoint => 15, // Harder as you need to complete the whole route
            _ => 10
        };
    }
}
