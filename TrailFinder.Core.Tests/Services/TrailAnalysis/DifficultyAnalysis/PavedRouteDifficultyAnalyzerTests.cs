// TrailFinder.UnitTests\DifficultyAnalyzers\PavedRouteDifficultyAnalyzerTests.cs

using FluentAssertions;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Services.TrailAnalysis.DifficultyAnalysis;
using TrailFinder.Core.ValueObjects;

public class PavedRouteDifficultyAnalyzerTests
{
    private readonly PavedRouteDifficultyAnalyzer _analyzer;

    public PavedRouteDifficultyAnalyzerTests()
    {
        _analyzer = new PavedRouteDifficultyAnalyzer();
    }

    [Theory]
    // Test cases for Paved routes
    // Remember the thresholds defined in PavedRouteDifficultyAnalyzer:
    // PavedShortDistance = 10000m (10km)
    // PavedModerateDistance = 21000m (21km)
    // PavedLongDistance = 42000m (42km)
    // PavedLowElevationGain = 50m
    // PavedModerateElevationGain = 150m
    // PavedHighElevationGain = 300m (Assuming you corrected this to be > Moderate)

    // And the score ranges for Paved:
    // < 15 => Easy
    // < 30 => Moderate
    // < 50 => Hard
    // _ => Extreme

    // Easy Paved (Score < 15)
    // Dist (9km): 5 pts, Elev (40m): 5 pts, Terrain (Flat): 5 pts, Route (Circular): 8 pts
    // Total: 5+5+5+8 = 23 (This would be Moderate based on the provided ranges)
    // Need to adjust the points to reach < 15 if you truly want "Easy".
    // For demonstration, let's assume Paved's base scores are lower for Easy.
    // If you adjust the score calculation methods within PavedRouteDifficultyAnalyzer
    // For example, if max points for Paved.Easy = 14:
    // Dist (9km): 5, Elev (40m): 5, Terrain (Flat): 0 (if terrain is almost irrelevant), Route (Circular): 4
    // Then 5+5+0+4 = 14 -> Easy.
    // Given the current base point structure, achieving "Easy" might be hard,
    // which indicates a tuning opportunity in the Paved analyzer's `CalculatePaved...Score` methods.
    // For now, I'll use values that fit the ranges as the code is currently written, assuming the thresholds *are* what you want.

    // Scenario 1: Moderate Paved (Score 15-29)
    // Example: 10km, 40m elevation, Flat, Circular
    // Dist (10km): 5, Elev (40m): 5, Terrain (Flat): 5, Route (Circular): 8
    // Total: 5 + 5 + 5 + 8 = 23 -> Moderate
    [InlineData(10000, 40, TerrainType.Flat, RouteType.Circular, DifficultyLevel.Moderate)]

    // Scenario 2: Hard Paved (Score 30-49)
    // Example: 25km, 100m elevation, Rolling, OutAndBack
    // Dist (25km): 25, Elev (100m): 15, Terrain (Rolling): 12, Route (OutAndBack): 10
    // Total: 25 + 15 + 12 + 10 = 62 -> Extreme (Oops, still hitting extreme)
    // Let's target 30-49 range.
    // Dist (15km): 15, Elev (80m): 15, Terrain (Flat): 5, Route (OutAndBack): 10
    // Total: 15 + 15 + 5 + 10 = 45 -> Hard
    [InlineData(15000, 80, TerrainType.Flat, RouteType.OutAndBack, DifficultyLevel.Hard)]

    // Scenario 3: Extreme Paved (Score 50-100)
    // Example: 45km, 200m elevation, Hilly, PointToPoint
    // Dist (45km): 30, Elev (200m): 23, Terrain (Hilly): 18, Route (PointToPoint): 15
    // Total: 30 + 23 + 18 + 15 = 86 -> Extreme
    [InlineData(45000, 200, TerrainType.Hilly, RouteType.PointToPoint, DifficultyLevel.Extreme)]
    public void Analyze_ReturnsCorrectDifficultyLevel_ForPavedRoutes(
        double distance,
        double elevationGain,
        TerrainType terrainType,
        RouteType routeType,
        DifficultyLevel expectedDifficulty)
    {
        // Arrange
        var input = DifficultyAnalysisInput.Builder()
            .WithTotalDistance(distance)
            .WithElevationGain(elevationGain)
            .WithTerrainType(terrainType)
            .WithRouteType(routeType)
            .Build();

        // Act
        var result = _analyzer.Analyze(input);

        // Assert
        result.Should().Be(expectedDifficulty);
    }
}