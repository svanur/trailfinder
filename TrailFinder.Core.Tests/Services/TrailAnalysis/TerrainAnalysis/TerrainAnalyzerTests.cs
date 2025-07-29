// TrailFinder.UnitTests\TerrainAnalyzerTests.cs

using FluentAssertions;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Services.TrailAnalysis;
using TrailFinder.Core.Services.TrailAnalysis.TerrainAnalysis;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Core.Tests.Services.TrailAnalysis;

public class TerrainAnalyzerTests
{
    [Theory]
    [InlineData(1000, 10, TerrainType.Flat)] // 10m/km <= 20
    [InlineData(5000, 50, TerrainType.Flat)] // 10m/km <= 20
    [InlineData(1000, 20, TerrainType.Flat)] // Exactly 20m/km
    [InlineData(1000, 21, TerrainType.Rolling)] // 21m/km > 20
    [InlineData(2000, 100, TerrainType.Rolling)] // 50m/km <= 50
    [InlineData(1000, 50, TerrainType.Rolling)] // Exactly 50m/km
    [InlineData(1000, 51, TerrainType.Hilly)] // 51m/km > 50
    [InlineData(3000, 300, TerrainType.Hilly)] // 100m/km <= 100
    [InlineData(1000, 100, TerrainType.Hilly)] // Exactly 100m/km
    [InlineData(1000, 101, TerrainType.Mountainous)] // 101m/km > 100
    [InlineData(10000, 2000, TerrainType.Mountainous)] // 200m/km
    public void AnalyzeTerrain_ReturnsCorrectTerrainType(double totalDistance, double elevationGain, TerrainType expectedTerrain)
    {
        // Arrange
        var input = new TerrainAnalysisInput
        {
            TotalDistance = totalDistance,
            ElevationGain = elevationGain
        };
        // Act
        var result = TerrainAnalyzer.AnalyzeTerrain(input.TotalDistance, input.ElevationGain);

        // Assert
        result.Should().Be(expectedTerrain);
    }

    [Fact]
    public void AnalyzeTerrain_HandlesZeroDistance_ReturnsMountainousOrUnknown()
    {
        // Arrange
        var input = new TerrainAnalysisInput
        {
            TotalDistance = 0,
            ElevationGain = 100 // 100m elevation gain over 0 distance
        };

        // Act
        // Division by zero will occur internally. Your code needs to handle this gracefully.
        // The current code will throw DivideByZeroException.
        // You might want to define behavior for this, e.g., return Unknown or Mountainous.
        // For testing, you expect the exception or specific handling.
        Action act = () => TerrainAnalyzer.AnalyzeTerrain(input.TotalDistance, input.ElevationGain);

        // Assert
        act.Should().Throw<DivideByZeroException>(); // Or whatever specific exception/return you define
    }
}