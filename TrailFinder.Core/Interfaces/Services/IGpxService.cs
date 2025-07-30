// TrailFinder.Core.Interfaces.Services\IGpxService.cs

using TrailFinder.Core.DTOs.Gpx.Responses;

namespace TrailFinder.Core.Interfaces.Services;

public interface IGpxService
{
    /// <summary>
    ///     Extracts GPX information from the provided stream.
    /// </summary>
    /// <param name="gpxStream">The stream containing the GPX data to be processed.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="GpxAnalysisResult" /> object
    ///     with details such as distance, elevation gain, and start point.
    /// </returns>
    Task<GpxAnalysisResult> AnalyzeGpxTrack(Stream gpxStream);
}