// TrailFinder.UnitTests\AnalysisServiceTests.cs

using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Services.TrailAnalysis;
using TrailFinder.Core.ValueObjects;
using TrailFinder.Infrastructure.Services;
// For specific DTOs like DifficultyAnalysisInput

namespace TrailFinder.Api.Tests.Services;

public class AnalysisServiceTests
{
    private readonly AnalysisService _analysisService;
    private readonly Mock<DifficultyAnalyzerFactory> _mockDifficultyAnalyzerFactory; // Mock the factory
    private readonly Mock<IAnalyzer<List<GpxPoint>, RouteType>> _mockRouteAnalyzer;

    private readonly Mock<IAnalyzer<DifficultyAnalysisInput, DifficultyLevel>>
        _mockSpecificDifficultyAnalyzer; // To mock the analyzer returned by the factory

    private readonly Mock<IAnalyzer<TerrainAnalysisInput, TerrainType>> _mockTerrainAnalyzer;

    public AnalysisServiceTests()
    {
        _mockRouteAnalyzer = new Mock<IAnalyzer<List<GpxPoint>, RouteType>>();
        _mockTerrainAnalyzer = new Mock<IAnalyzer<TerrainAnalysisInput, TerrainType>>();
        _mockDifficultyAnalyzerFactory = new Mock<DifficultyAnalyzerFactory>(
            new ServiceCollection().BuildServiceProvider()); // Pass a dummy service provider

        // This mock will represent the *specific* analyzer that the factory returns
        _mockSpecificDifficultyAnalyzer = new Mock<IAnalyzer<DifficultyAnalysisInput, DifficultyLevel>>();

        // Setup the factory to return our specific mock analyzer when GetAnalyzer is called
        _mockDifficultyAnalyzerFactory.Setup(f => f.GetAnalyzer(It.IsAny<SurfaceType>()))
            .Returns(_mockSpecificDifficultyAnalyzer.Object);

        _analysisService = new AnalysisService(
            //new NullLogger<AnalysisService>(), // Use NullLogger for unit tests
            _mockRouteAnalyzer.Object,
            _mockTerrainAnalyzer.Object,
            _mockDifficultyAnalyzerFactory.Object
        );
    }

    // Existing tests for CalculateTotalDistance and CalculateElevationGain are still valid
    // as those methods are private and tested indirectly via the AnalyzeGpxPointsBySurfaceType method's results.

    [Fact]
    public void Analyze_PassesCorrectDataAndSurfaceTypeToAnalyzers()
    {
        // Arrange
        var points = new List<GpxPoint>
        {
            new(0, 0, 0),
            new(0.000008983, 0, 10), // A small distance, some elevation
            new(0.000017966, 0, 20)
        };
        // Expected total distance is 2m, elevation gain 20m (based on previous calculation)
        var expectedTotalDistance = 2.0;
        var expectedElevationGain = 20.0;
        var surfaceType = SurfaceType.Trail; // Simulate a determined surface type

        // Define what the mock sub-analyzers *should* return
        _mockRouteAnalyzer.Setup(a => a.Analyze(points)).Returns(RouteType.Circular);
        _mockTerrainAnalyzer.Setup(a =>
            a.Analyze(It.Is<TerrainAnalysisInput>(input =>
                input.TotalDistance == expectedTotalDistance && input.ElevationGain == expectedElevationGain
            ))).Returns(TerrainType.Mountainous);

        // Define what the *specific* difficulty analyzer should return when called
        _mockSpecificDifficultyAnalyzer.Setup(a => a.Analyze(It.Is<DifficultyAnalysisInput>(input =>
            input.TotalDistance == expectedTotalDistance &&
            input.ElevationGain == expectedElevationGain &&
            input.TerrainType == TerrainType.Mountainous &&
            input.RouteType == RouteType.Circular
        ))).Returns(DifficultyLevel.Extreme);

        // Act
        var result = _analysisService.AnalyzeGpxPointsBySurfaceType(points, surfaceType); // Pass the surfaceType

        // Assert
        result.Should().NotBeNull();
        result.TotalDistance.Should().Be(expectedTotalDistance);
        result.TotalElevationGain.Should().Be(expectedElevationGain);
        result.RouteType.Should().Be(RouteType.Circular);
        result.TerrainType.Should().Be(TerrainType.Mountainous);
        result.DifficultyLevel.Should().Be(DifficultyLevel.Extreme);

        // Verify that the factory was asked for the correct analyzer
        _mockDifficultyAnalyzerFactory.Verify(f => f.GetAnalyzer(surfaceType), Times.Once);

        // Verify that the correct specific difficulty analyzer's AnalyzeGpxPointsBySurfaceType method was called
        _mockSpecificDifficultyAnalyzer.Verify(a => a.Analyze(It.Is<DifficultyAnalysisInput>(input =>
            input.TotalDistance == expectedTotalDistance &&
            input.ElevationGain == expectedElevationGain &&
            input.TerrainType == TerrainType.Mountainous &&
            input.RouteType == RouteType.Circular
        )), Times.Once);

        // Verify other analyzer calls
        _mockRouteAnalyzer.Verify(a => a.Analyze(points), Times.Once);
        _mockTerrainAnalyzer.Verify(a =>
            a.Analyze(It.Is<TerrainAnalysisInput>(input =>
                input.TotalDistance == expectedTotalDistance && input.ElevationGain == expectedElevationGain
            )), Times.Once);
    }

    // Test the elevation gain logic if the smoothing was there
    // For the current CalculateElevationGain:
    [Fact]
    public void CalculateElevationGain_HandlesFlatRoute()
    {
        var points = new List<GpxPoint>
        {
            new(0, 0, 100), new(0, 0, 100), new(0, 0, 100)
        };
        var result =
            InvokePrivateMethod<double>(_analysisService, "CalculateElevationGain", points.Select(p => p.Elevation));
        result.Should().Be(0);
    }

    [Fact]
    public void CalculateElevationGain_HandlesMixedAscentAndDescent()
    {
        var points = new List<GpxPoint>
        {
            new(0, 0, 100), new(0, 0, 110), // +10
            new(0, 0, 105), // -5 (no gain)
            new(0, 0, 120), // +15
            new(0, 0, 115) // -5 (no gain)
        };
        // Expected: 10 + 15 = 25
        var result =
            InvokePrivateMethod<double>(_analysisService, "CalculateElevationGain", points.Select(p => p.Elevation));
        result.Should().Be(25);
    }

    [Fact]
    public void CalculateTotalDistance_HandlesStraightLine()
    {
        // Approx 111 meters between (0,0) and (0, 0.001)
        var points = new List<GpxPoint>
        {
            new(0, 0, 0),
            new(0, 0.001, 0)
        };
        var result = InvokePrivateMethod<double>(_analysisService, "CalculateTotalDistance", points);
        result.Should().Be(111); // Approx, will be rounded
    }

    // Helper method to invoke private methods for testing.
    // Generally, try to test public API, but for private calculation methods like these,
    // this can be acceptable if they contain complex logic worth direct testing.
    // If these were separate internal/public utility methods, you'd test them directly.
    private T InvokePrivateMethod<T>(object instance, string methodName, params object[] parameters)
    {
        var method = instance.GetType()
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        if (method == null) throw new ArgumentException($"Method '{methodName}' not found.");
        return (T)method.Invoke(instance, parameters);
    }
}