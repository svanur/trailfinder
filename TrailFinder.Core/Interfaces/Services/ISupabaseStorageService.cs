namespace TrailFinder.Core.Interfaces.Services;

public interface ISupabaseStorageService
{

    /// <summary>
    ///     Retrieves the GPX file stream from the storage based on the specified trail identifier.
    /// </summary>
    /// <param name="trailId">The unique identifier of the trail for which the GPX file is to be retrieved.</param>
    /// <param name="trailSlug">The slug of the trail.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a stream of the GPX file
    ///     associated with the given trail identifier.
    /// </returns>
    Task<Stream> GetGpxFileFromStorage(Guid trailId, string trailSlug);

    Task<bool> UploadGpxFileAsync(Guid trailId, string trailSlug, Stream fileStream, string fileName);

    Task<Stream> DownloadGpxFileAsync(Guid trailId, string trailSlug);
}