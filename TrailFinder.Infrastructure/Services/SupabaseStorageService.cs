using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Supabase;
using Supabase.Storage;
using Supabase.Storage.Interfaces;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Infrastructure.Configuration;
using Client = Supabase.Client;

namespace TrailFinder.Infrastructure.Services;

public class SupabaseStorageService : ISupabaseStorageService
{
    private readonly Client _supabaseClient; //  ensure it's registered as such in the DI container
    private const string BucketName = "gpx-files";
    private readonly ILogger<SupabaseStorageService> _logger;

    public IStorageBucketApi<Bucket> Storage => _supabaseClient.Storage;
    public IStorageFileApi<FileObject> From(string bucket) => _supabaseClient.Storage.From(bucket);

    public SupabaseStorageService(
        IOptions<SupabaseSettings> settings, 
        ILogger<SupabaseStorageService> logger
    )
    {
        _logger = logger;
        var options = new SupabaseOptions
        {
            // If using only Storage and not Realtime subscriptions, this might be unnecessary overhead.
            // Consider setting it to false if Realtime isn't used by the Storage service.
            AutoRefreshToken = false,
            AutoConnectRealtime = true
        };

        _supabaseClient = new Client(
            settings.Value.Url,
            settings.Value.Key,
            options);
    }
    
    public async Task<bool> UploadGpxFileAsync(Guid trailId, string trailSlug, Stream fileStream, string fileName)
    {
        var filePath = $"{trailSlug}/{trailId}/{fileName}";
        
        try 
        {
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            
            var uploadResult = await From(BucketName)
                .Upload(fileBytes, filePath);

            return !string.IsNullOrEmpty(uploadResult);
        }
        catch (Exception ex)
        {
            // Inject ILogger into SupabaseStorageService
            _logger.LogError(ex, "Failed to upload GPX file for trail {TrailId} to path {FilePath}", trailId, filePath);
            return false;
        }
    }
    
    public async Task<(Stream? fileStream, string? fileName)> DownloadGpxFileAsync(string storagePath)
    {
        try
        {
            var bytes = await From(BucketName).Download(storagePath, null);

            if (bytes.Length == 0)
            {
                // This indicates file not found in storage, which is a common scenario.
                // Don't throw FileNotFoundException here if you're returning null.
                // Throw if you consider an empty file an error, otherwise return null stream.
                return (null, null);
            }

            var stream = new MemoryStream(bytes);

            // Extract filename from storagePath for consistency.
            // Or, better, the calling code (controller) will use the original_file_name from DB metadata.
            // This returned fileName might not be strictly needed by the controller anymore.
            var retrievedFileName = Path.GetFileName(storagePath);

            return (stream, retrievedFileName);
        }
        catch (Exception ex)
        {
            // Log the actual exception details here
            _logger.LogError(ex, $"Error downloading GPX file from storage at path: {storagePath}");
            // Re-throw if it's a critical error, or return (null, null) based on your error strategy
            return (null, null); // Return nulls on error, let the caller handle NotFound
        }
    }
}
