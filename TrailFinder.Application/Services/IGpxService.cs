using TrailFinder.Core.DTOs.Gpx;

namespace TrailFinder.Application.Services;

public interface IGpxService
{
    /// <summary>
    ///     Extracts GPX information from the provided stream.
    /// </summary>
    /// <param name="gpxStream">The stream containing the GPX data to be processed.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="GpxInfoDto" /> object
    ///     with details such as distance, elevation gain, and start point.
    /// </returns>
    Task<GpxInfoDto> ExtractGpxInfo(Stream gpxStream);

    /// <summary>
    ///     Retrieves the GPX file stream from the storage based on the specified trail identifier.
    /// </summary>
    /// <param name="trailId">The unique identifier of the trail for which the GPX file is to be retrieved.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a stream of the GPX file
    ///     associated with the given trail identifier.
    /// </returns>
    Task<Stream> GetGpxFileFromStorage(Guid trailId);
}
