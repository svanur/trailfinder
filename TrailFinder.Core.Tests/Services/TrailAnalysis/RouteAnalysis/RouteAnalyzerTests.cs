// TrailFinder.UnitTests\RouteAnalyzerTests.cs

using FluentAssertions;
using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Services.TrailAnalysis.RouteAnalysis; // Assuming this is where RouteAnalyzer is

namespace TrailFinder.UnitTests.Services.TrailAnalysis; // Adjust namespace if needed

public class RouteAnalyzerTests
{
    private readonly RouteAnalyzer _analyzer; // Instance of the analyzer

    public RouteAnalyzerTests()
    {
        _analyzer = new RouteAnalyzer(); // Instantiate the analyzer
    }

   [Fact(Skip="for now")]
    public void Analyze_ReturnsCircular_WhenStartAndEndAreNearby()
    {
        // Arrange
        var points = new List<GpxPoint>
        {
            new(0, 0, 0),
            new(0, 0.0001, 10), // Small step
            new(0.0001, 0.0001, 20), // Small step
            new(0.0000005, 0.0000005, 0) // Very close to start (within ~1 meter of 0,0)
        };

        // Act
        var result = _analyzer.Analyze(points); // Call instance method

        // Assert
        result.Should().Be(RouteType.Circular);
    }

   [Fact(Skip="for now")]
    public void Analyze_ReturnsOutAndBack_WhenPathIsSimilarInReverseAndNotCircular()
    {
        // Arrange - Create an out-and-back like path
        // Ensure the last point is *not* nearby the first for circular test,
        // but the path's shape indicates a return.
        var points = new List<GpxPoint>
        {
            new(0, 0, 0),       // Start
            new(0.001, 0, 10),  // Go North
            new(0.002, 0, 20),  // Go further North (turnaround point area)
            new(0.0019, 0, 19), // Start returning South
            new(0.001, 0, 10),  // Continue returning South
            new(0.0002, 0, 2)   // End point - relatively close to start, but *outside* circular threshold
                                // (e.g., 20-30 meters away if threshold is 50m)
        };
        
        // For the above example, if the circular threshold is 50m:
        // Distance from (0,0) to (0.0002, 0) is ~22.2 meters. This would cause it to be CIRCULAR!
        // You need to make the END point sufficiently far from the START point for this test to be valid.

        // Let's create a more distinct out-and-back where end is NOT near start for Circular check
        var outAndBackPoints = new List<GpxPoint>
        {
            new(0, 0, 0),               // A
            new(0.0005, 0.0005, 10),    // B
            new(0.001, 0.001, 20),      // C (approximate turnaround)
            new(0.0009, 0.0009, 19),    // C' (coming back)
            new(0.0006, 0.0006, 11),    // B'
            new(0.0003, 0.0003, 3)      // A'' - end point, not near A, but indicates return
        };
        // Distance from (0,0) to (0.0003, 0.0003) is roughly 47 meters.
        // If CircularThresholdMeters is 50, this would still be CIRCULAR.
        // Let's make it clearly NOT circular by making the end point further away
        // Or ensure the IsOutAndBack logic is checked AFTER IsCircular and it's robust.

        // To guarantee it's not circular, the start and end must be further apart than CircularThresholdMeters (e.g., 50m)
        // Let's try an end point that is, say, 70-80 meters away from start.
        // 0.0005 degrees of latitude is ~55 meters.
        var idealOutAndBackPoints = new List<GpxPoint>
        {
            new(0, 0, 0),               // Start
            new(0.001, 0.001, 10),      // Middle 1
            new(0.002, 0.002, 20),      // Middle 2 (Turnaround point)
            new(0.0018, 0.0018, 18),    // Return 1
            new(0.001, 0.001, 10),      // Return 2
            new(0.0005, 0.0005, 5)      // End point. Distance from (0,0) to (0.0005, 0.0005) is ~78m.
                                        // This should be *outside* a 50m CircularThreshold.
        };


        // Act
        var result = _analyzer.Analyze(idealOutAndBackPoints);

        // Assert
        result.Should().Be(RouteType.OutAndBack);
    }

    // You might combine the "OutAndAlmostBack" into the main one, or have more specific tests
    // for edge cases of the out-and-back algorithm.

   [Fact(Skip="for now")]
    public void Analyze_ReturnsPointToPoint_WhenNeitherCircularNorOutAndBack()
    {
        // Arrange - A clear point-to-point path
        var points = new List<GpxPoint>
        {
            new(0, 0, 0),
            new(0.01, 0.01, 50), // Significant distance
            new(0.02, 0.02, 100)
        };

        // Act
        var result = _analyzer.Analyze(points);

        // Assert
        result.Should().Be(RouteType.PointToPoint);
    }

   [Fact(Skip="for now")]
    public void Analyze_ReturnsUnknown_ForTooFewPoints()
    {
        // Arrange
        var points = new List<GpxPoint>
        {
            new(0, 0, 0) // Only one point
        };

        // Act
        var result = _analyzer.Analyze(points);

        // Assert
        result.Should().Be(RouteType.Unknown);
    }
    
   [Fact(Skip="for now")]
    public void Analyze_ReturnsUnknown_ForEmptyPointsList()
    {
        // Arrange
        var points = new List<GpxPoint>();

        // Act
        var result = _analyzer.Analyze(points);

        // Assert
        result.Should().Be(RouteType.Unknown);
    }
}