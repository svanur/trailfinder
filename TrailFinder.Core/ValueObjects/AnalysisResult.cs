using TrailFinder.Core.DTOs.Gpx;
using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.ValueObjects;

public class AnalysisResult
{
    public double TotalDistance { get; }
    public double TotalElevationGain { get; }
    public double TotalElevationLoss { get; }
    public double VerticalRatio { get; }
    public RouteType RouteType { get; }
    public TerrainType TerrainType { get; }
    public DifficultyLevel DifficultyLevel { get; }
    public GpxPoint StartGpxPoint { get; }
    public GpxPoint EndGpxPoint { get; }

    public AnalysisResult(
        double totalTotalDistance,
        double totalElevationGain,
        RouteType routeType, 
        TerrainType terrainType, 
        DifficultyLevel difficultyLevel,
        GpxPoint startGpxPoint,
        GpxPoint endGpxPoint
    )
    {
        TotalDistance = totalTotalDistance;
        TotalElevationGain = totalElevationGain;
        RouteType = routeType;
        TerrainType = terrainType;
        DifficultyLevel = difficultyLevel;
        StartGpxPoint = startGpxPoint;
        EndGpxPoint = endGpxPoint;
    }
}