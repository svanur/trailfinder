using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;
using TrailFinder.Core.ValueObjects;

namespace TrailFinder.Core.Interfaces.Services;

public interface IAnalysisService
{
    /// <summary>
    /// Analyzes a list of GPX points to determine specific statistics and characteristics
    /// associated with a given surface type.
    /// </summary>
    /// <param name="points">A list of GPX points representing a trail or route.</param>
    /// <param name="surfaceType">The surface type (e.g., trail, paved, mixed) to analyze the GPX points for.</param>
    /// <returns>An object containing analysis results, such as total distance, elevation gain,
    /// and other route and terrain details.</returns>
    AnalysisResult AnalyzeGpxPointsBySurfaceType(List<GpxPoint> points, SurfaceType surfaceType);
    
}
