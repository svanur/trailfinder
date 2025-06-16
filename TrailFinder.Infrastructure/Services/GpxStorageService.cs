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

    public async Task<bool> UploadGpxFileAsync(Guid trailId, string trailSlug, Stream fileStream, string fileName)
    {
        var filePath = $"{trailSlug}/{trailId}/{fileName}";
        
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

   
    public async Task<Stream> DownloadGpxFileAsync(Guid trailId)
    {
        // First, we need to get the trail slug to construct the correct path
        var trail = await _supabaseService.GetTrailByIdAsync(trailId);
        if (trail == null)
        {
            throw new FileNotFoundException($"Trail not found with ID {trailId}");
        }

        var filePath = $"{trail.Slug}/{trailId}.gpx";
        try
        {
            var bytes = await _supabaseService
                .From(BucketName)
                .Download(filePath, null);

            if (bytes == null || bytes.Length == 0)
            {
                throw new FileNotFoundException($"GPX file not found at path {filePath}");
            }

            return new MemoryStream(bytes);
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Error accessing GPX file for trail {trailId}: {ex.Message}", ex);
        }
    }

    public Task<bool> DeleteGpxFileAsync(Guid trailId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> FileExistsAsync(Guid trailId)
    {
        throw new NotImplementedException();
    }
}
