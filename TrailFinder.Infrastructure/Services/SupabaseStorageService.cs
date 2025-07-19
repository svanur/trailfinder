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
    
    public async Task<(Stream? fileStream, string? fileName)> DownloadGpxFileAsync(Guid trailId, string trailSlug)
    {
        var filePath = $"{trailSlug}/{trailId}.gpx";
        try
        {
            var bytes = await From(BucketName)
                .Download(filePath, null);

            if (bytes == null || bytes.Length == 0)
            {
                throw new FileNotFoundException($"GPX file not found at path {filePath}");
            }

            var stream = new MemoryStream(bytes);

            // Or get from storage metadata when available
            var retrievedFileName = trailSlug + "/" + trailId + ".gpx"; 

            return (stream, retrievedFileName);
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Error accessing GPX file for trail {trailId}: {ex.Message}", ex);
        }
    }
}
