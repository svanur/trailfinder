// 1. Define a new interface (e.g., in Core/Interfaces)
// TrailFinder.Core.Interfaces.Services\IOsmLookupService.cs (New file)

using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;

namespace TrailFinder.Core.Interfaces.Services;

public interface IOsmLookupService
{
    /// <summary>
    ///     Attempts to determine the SurfaceType of a route based on its GPS points.
    /// </summary>
    /// <param name="gpxPoints">The list of GPX points representing the route.</param>
    /// <returns>The determined SurfaceType, or Unknown if it cannot be determined.</returns>
    Task<SurfaceType> DetermineSurfaceType(List<GpxPoint> gpxPoints);
}