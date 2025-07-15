using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.ValueObjects;

public class AnalysisResult
{
    public double Distance { get; }
    public double ElevationGain { get; }
    public RouteType RouteType { get; }
    public TerrainType TerrainType { get; }
    public DifficultyLevel DifficultyLevel { get; }
    public GpxPoint StartGpxPoint { get; }
    public GpxPoint EndGpxPoint { get; }

    public AnalysisResult(
        double totalDistance,
        double elevationGain,
        RouteType routeType, 
        TerrainType terrainType, 
        DifficultyLevel difficultyLevel,
        GpxPoint startGpxPoint,
        GpxPoint endGpxPoint
    )
    {
        Distance = totalDistance;
        ElevationGain = elevationGain;
        RouteType = routeType;
        TerrainType = terrainType;
        DifficultyLevel = difficultyLevel;
        StartGpxPoint = startGpxPoint;
        EndGpxPoint = endGpxPoint;
    }
}