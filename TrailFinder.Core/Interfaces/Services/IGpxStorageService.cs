namespace TrailFinder.Core.Interfaces.Services;

public interface IGpxStorageService
{
    Task<bool> UploadGpxFileAsync(Guid trailId, string trailSlug, Stream fileStream, string fileName); //TODO: we need the slug here also, for the folder name 
    Task<Stream> DownloadGpxFileAsync(Guid trailId);
    Task<bool> DeleteGpxFileAsync(Guid trailId);
    Task<bool> FileExistsAsync(Guid trailId);
}
