namespace TrailFinder.Core.Interfaces.Services;

public interface IGpxStorageService
{
    Task<bool> UploadGpxFileAsync(Guid trailId, Stream fileStream, string fileName); 
    Task<Stream> DownloadGpxFileAsync(Guid trailId);
    Task<bool> DeleteGpxFileAsync(Guid trailId);
    Task<bool> FileExistsAsync(Guid trailId);
}
