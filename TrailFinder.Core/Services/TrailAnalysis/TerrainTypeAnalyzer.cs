using TrailFinder.Core.Enums;

namespace TrailFinder.Infrastructure.Analyzers;

public class TerrainAnalyzer
{
    // Thresholds for elevation gain per kilometer
    private const double FlatThreshold = 20;        // 20m per km
    private const double RollingThreshold = 50;     // 50m per km
    private const double HillyThreshold = 100;      // 100m per km
    // Above 100m/km = Mountainous

    public static TerrainType AnalyzeTerrain(double totalDistance, double ElevationGain)
    {
        // Convert to kilometers for easier threshold comparison
        var distanceKm = totalDistance / 1000;
        
        // Calculate elevation gain per kilometer
        var gainPerKm = ElevationGain / distanceKm;

        return gainPerKm switch
        {
            <= FlatThreshold => TerrainType.Flat,
            <= RollingThreshold => TerrainType.Rolling,
            <= HillyThreshold => TerrainType.Hilly,
            _ => TerrainType.Mountainous
        };
    }
}