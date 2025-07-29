// TrailFinder.UnitTests\RouteAnalyzerTests.cs

using FluentAssertions;
using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Services.TrailAnalysis.RouteAnalysis;

namespace TrailFinder.Core.Tests.Services.TrailAnalysis;

public class RouteAnalyzerTests
{
    // The RouteAnalyzer uses static methods, so we can test them directly.
    // If it were an instance method (non-static), you'd instantiate it:
    // private readonly RouteAnalyzer _analyzer = new RouteAnalyzer();

    [Fact]
    public void DetermineRouteType_ReturnsCircular_WhenStartAndEndAreNearby()
    {
        // Arrange
        var points = new List<GpxPoint>
        {
            new(0, 0, 0),
            new(0, 0.001, 10),
            new(0.001, 0.001, 20),
            new(0.0000001, 0.0000001, 0) // Very close to start
        };

        // Act
        var result = RouteAnalyzer.DetermineRouteType(points);

        // Assert
        result.Should().Be(RouteType.Circular);
    }

    [Fact]
    public void DetermineRouteType_ReturnsOutAndBack_WhenPathIsSimilarInReverse()
    {
        // Arrange - Create an out-and-back like path
        var points = new List<GpxPoint>
        {
            new(0, 0, 0),
            new(1, 0, 10),
            new(2, 0, 20),
            new(1, 0, 10), // Returning
            new(0, 0.0001, 0) // Close to start, but not circular.
            // The IsOutAndBack logic needs to be robust here.
            // Make sure the last point is NOT nearby the first for circular test.
        };


        // Act
        var result = RouteAnalyzer.DetermineRouteType(points);

        // Assert
        result.Should().Be(RouteType.OutAndBack);
    }

    [Fact]
    public void DetermineRouteType_ReturnsOutAndAlmostBack_WhenPathIsSimilarInReverse()
    {
        // Arrange

        // Ensure the end is not close to the start
        // and the middle section reverses.
        var outAndBackPoints = new List<GpxPoint>
        {
            new(0, 0, 0),
            new(1, 1, 10),
            new(2, 2, 20),
            new(1.9, 2.1, 19), // Close to (2,2) for the reverse path
            new(0.9, 1.1, 9), // Close to (1,1) for the reverse path
            new(0.1, 0.1, 1) // Not extremely close to start (0,0)
        };


        // Act
        var result = RouteAnalyzer.DetermineRouteType(outAndBackPoints);

        // Assert
        result.Should().Be(RouteType.OutAndBack);
    }

    [Fact]
    public void DetermineRouteType_ReturnsPointToPoint_WhenNeitherCircularNorOutAndBack()
    {
        // Arrange - A clear point-to-point path
        var points = new List<GpxPoint>
        {
            new(0, 0, 0),
            new(10, 10, 50),
            new(20, 20, 100)
        };

        // Act
        var result = RouteAnalyzer.DetermineRouteType(points);

        // Assert
        result.Should().Be(RouteType.PointToPoint);
    }

    [Fact]
    public void DetermineRouteType_ReturnsUnknown_ForTooFewPoints()
    {
        // Arrange
        var points = new List<GpxPoint>
        {
            new(0, 0, 0) // Only one point
        };

        // Act
        var result = RouteAnalyzer.DetermineRouteType(points);

        // Assert
        result.Should().Be(RouteType.Unknown);
    }
}