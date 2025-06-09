using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Infrastructure.Services;

// TrailFinder.Infrastructure/Services/SupabaseGpxStorageService.cs
public class SupabaseGpxStorageService : IGpxStorageService
{
    private readonly ISupabaseService _supabaseService;
    private const string BucketName = "gpx-files";

    public SupabaseGpxStorageService(ISupabaseService supabaseService)
    {
        _supabaseService = supabaseService;
    }

    public async Task<bool> UploadGpxFileAsync(string trailId, Stream fileStream, string fileName)
    {
        var filePath = $"{trailId}/{fileName}";
        
        try 
        {
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var uploadResult = await _supabaseService
                .From(BucketName)
                .Upload(fileBytes, filePath);

            // If we got here without an exception and have a result, the upload was successful
            return !string.IsNullOrEmpty(uploadResult);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Task<Stream> DownloadGpxFileAsync(string trailId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteGpxFileAsync(string trailId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> FileExistsAsync(string trailId)
    {
        throw new NotImplementedException();
    }
}
