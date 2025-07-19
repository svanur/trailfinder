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
    /// Downloads a GPX file from the storage service using the specified storage path.
    /// </summary>
    /// <param name="storagePath">The path in the storage where the GPX file is located.</param>
    /// <returns>
    /// Returns a tuple containing the file's stream and its name. The stream is null if the file is not found.
    /// </returns>
    Task<(Stream? fileStream, string? fileName)> DownloadGpxFileAsync(string storagePath);
}