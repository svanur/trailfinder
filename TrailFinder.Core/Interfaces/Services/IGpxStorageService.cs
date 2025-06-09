namespace TrailFinder.Core.Interfaces.Services;

public interface IGpxStorageService
{
    Task<bool> UploadGpxFileAsync(string trailId, Stream fileStream, string fileName);
    Task<Stream> DownloadGpxFileAsync(string trailId);
    Task<bool> DeleteGpxFileAsync(string trailId);
    Task<bool> FileExistsAsync(string trailId);
}
