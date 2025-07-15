using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Core.Services.TrailAnalysis;

public class TerrainAnalyzer : IAnalyzer<TerrainAnalysisInput, TerrainType>
{
    // Thresholds for elevation gain per kilometer
    private const double FlatThreshold = 20;        // 20m per km
    private const double RollingThreshold = 50;     // 50m per km
    private const double HillyThreshold = 100;      // 100m per km
    // Above 100m/km = Mountainous

    public TerrainType Analyze(TerrainAnalysisInput item)
    {
        return AnalyzeTerrain(item.TotalDistance, item.ElevationGain);
    }
    
    public static TerrainType AnalyzeTerrain(double totalDistance, double elevationGain)
    {
        // Convert to kilometers for easier threshold comparison
        var distanceKm = totalDistance / 1000;
        
        // Calculate elevation gain per kilometer
        var gainPerKm = elevationGain / distanceKm;

        return gainPerKm switch
        {
            <= FlatThreshold => TerrainType.Flat,
            <= RollingThreshold => TerrainType.Rolling,
            <= HillyThreshold => TerrainType.Hilly,
            _ => TerrainType.Mountainous
        };
    }
}