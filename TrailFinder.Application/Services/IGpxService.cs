using TrailFinder.Core.DTOs.Gpx;

namespace TrailFinder.Application.Services;

public interface IGpxService
{
    /// <summary>
    ///     Extracts GPX information from the provided stream.
    /// </summary>
    /// <param name="gpxStream">The stream containing the GPX data to be processed.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="TrailGpxInfoDto" /> object
    ///     with details such as distance, elevation gain, and start point.
    /// </returns>
    Task<TrailGpxInfoDto> ExtractGpxInfo(Stream gpxStream);
}

