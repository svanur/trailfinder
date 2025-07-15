using TrailFinder.Core.Enums;

namespace TrailFinder.Core.ValueObjects;

public class AnalysisResult
{
    public RouteType RouteType { get; }
    public TerrainType TerrainType { get; }
    public DifficultyLevel DifficultyLevel { get; }

    public AnalysisResult(RouteType routeType, TerrainType terrainType, DifficultyLevel difficultyLevel)
    {
        RouteType = routeType;
        TerrainType = terrainType;
        DifficultyLevel = difficultyLevel;
    }
}