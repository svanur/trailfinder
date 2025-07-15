using System.Drawing;
using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Infrastructure.Services;


public class AnalysisService
{
    private readonly IAnalyzer<List<GpxPoint>, RouteType> _routeAnalyzer;
    private readonly IAnalyzer<TerrainAnalysisInput, TerrainType> _terrainAnalyzer;
    private readonly IAnalyzer<DifficultyAnalysisInput, DifficultyLevel> _difficultyAnalyzer;

    // Constructor remains the same
    public AnalysisService(
        IAnalyzer<List<GpxPoint>, RouteType> routeAnalyzer,
        IAnalyzer<TerrainAnalysisInput, TerrainType> terrainAnalyzer,
        IAnalyzer<DifficultyAnalysisInput, DifficultyLevel> difficultyAnalyzer)
    {
        _routeAnalyzer = routeAnalyzer;
        _terrainAnalyzer = terrainAnalyzer;
        _difficultyAnalyzer = difficultyAnalyzer;
    }

    public AnalysisResult Analyze(List<GpxPoint> points, double totalDistance, double elevationGain)
    {
        var routeType = _routeAnalyzer.Analyze(points);

        // Using object initializer for TerrainAnalysisInput (simple, no builder really needed for just 2 props)
        var terrainInput = new TerrainAnalysisInput
        {
            TotalDistance = totalDistance,
            ElevationGain = elevationGain
        };
        var terrainType = _terrainAnalyzer.Analyze(terrainInput);

        // Using the Builder pattern for DifficultyAnalysisInput
        var difficultyInput = DifficultyAnalysisInput.Builder()
            .WithTotalDistance(totalDistance)
            .WithElevationGain(elevationGain)
            .WithTerrainType(terrainType)
            .WithRouteType(routeType)
            .Build();

        var difficultyLevel = _difficultyAnalyzer.Analyze(difficultyInput);

        return new AnalysisResult(routeType, terrainType, difficultyLevel);
    }
}