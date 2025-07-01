using TrailFinder.Core.Enums;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Core.Builders;

// Builders/DifficultyAnalysisInputBuilder.cs
public class DifficultyAnalysisInputBuilder
{
    private double _totalDistance;
    private double _elevationGain;
    private TerrainType _terrainType;
    private RouteType _routeType;

    public DifficultyAnalysisInputBuilder WithTotalDistance(double totalDistance)
    {
        _totalDistance = totalDistance;
        return this; // Return 'this' for method chaining
    }

    public DifficultyAnalysisInputBuilder WithElevationGain(double elevationGain)
    {
        _elevationGain = elevationGain;
        return this;
    }

    public DifficultyAnalysisInputBuilder WithTerrainType(TerrainType terrainType)
    {
        _terrainType = terrainType;
        return this;
    }

    public DifficultyAnalysisInputBuilder WithRouteType(RouteType routeType)
    {
        _routeType = routeType;
        return this;
    }

    public DifficultyAnalysisInput Build()
    {
        // Add any validation here before creating the object
        // For example:
        if (_totalDistance < 0) throw new ArgumentOutOfRangeException(nameof(_totalDistance), "Total distance cannot be negative.");

        return new DifficultyAnalysisInput(
            _totalDistance,
            _elevationGain,
            _terrainType,
            _routeType
        );
    }
}