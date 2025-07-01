using TrailFinder.Core.Builders;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.ValueObjects;

// DTOs/DifficultyAnalysisInput.cs
public class DifficultyAnalysisInput
{
    public double TotalDistance { get; }
    public double ElevationGain { get; }
    public TerrainType TerrainType { get; }
    public RouteType RouteType { get; }

    // Private constructor, only accessible by the Builder
    internal DifficultyAnalysisInput(
        double totalDistance,
        double elevationGain,
        TerrainType terrainType,
        RouteType routeType
    )
    {
        TotalDistance = totalDistance;
        ElevationGain = elevationGain;
        TerrainType = terrainType;
        RouteType = routeType;
    }

    // Static method to get a new builder instance
    public static DifficultyAnalysisInputBuilder Builder()
    {
        return new DifficultyAnalysisInputBuilder();
    }
}