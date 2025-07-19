namespace TrailFinder.Core.Interfaces.Services;

public interface ISupabaseStorageService
{
    /// <summary>
    /// Uploads a GPX file to the storage service with the specified trail details.
    /// </summary>
    /// <param name="trailId">The unique identifier of the trail.</param>
    /// <param name="trailSlug">The slug representing the trail.</param>
    /// <param name="fileStream">The stream of the file to be uploaded.</param>
    /// <param name="fileName">The name of the file to be uploaded.</param>
    /// <returns>Returns a boolean value indicating whether the upload was successful.</returns>
    Task<bool> UploadGpxFileAsync(Guid trailId, string trailSlug, Stream fileStream, string fileName);

    /// <summary>
    /// Downloads a GPX file from the storage service associated with the specified trail details.
    /// </summary>
    /// <param name="trailId">The unique identifier of the trail.</param>
    /// <param name="trailSlug">The slug representing the trail.</param>
    /// <returns>Returns a tuple containing a stream of the file and its name, or null values if the file is not found.</returns>
    Task<(Stream? fileStream, string? fileName)> DownloadGpxFileAsync(Guid trailId, string trailSlug);
}