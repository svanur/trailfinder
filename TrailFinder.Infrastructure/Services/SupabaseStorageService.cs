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
    private readonly Client _supabaseClient;
    private const string BucketName = "gpx-files";
    
    public IStorageBucketApi<Bucket> Storage => _supabaseClient.Storage;
    public IStorageFileApi<FileObject> From(string bucket) => _supabaseClient.Storage.From(bucket);

    public SupabaseStorageService(IOptions<SupabaseSettings> settings)
    {
        var options = new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        };

        _supabaseClient = new Client(
            settings.Value.Url,
            settings.Value.Key,
            options);
    }
    
    
    public async Task<Stream> GetGpxFileFromStorage(Guid trailId, string trailSlug)
    {
        var fileName = $"{trailSlug}/{trailId}.gpx";

        try
        {
            // First, check if the bucket exists
            /*
            var buckets = await _storageService
                .Storage
                .ListBuckets();

            if (buckets != null && buckets.All(b => b.Name != BucketName))
            {
                await _storageService
                    .Storage
                    .CreateBucket(BucketName);
            }

            // List files in the bucket to check if our file exists
            var files = await _storageService
                .From(BucketName)
                .List();

            if (files != null && files.All(f => f.Name != fileName))
            {
                throw new FileNotFoundException($"GPX file {fileName} not found in storage");
            }
            */

            var response = await From(BucketName)
                .Download(fileName, null);

            if (response == null || response.Length == 0)
                throw new InvalidOperationException($"Downloaded file {fileName} is empty");

            return new MemoryStream(response);
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Error accessing GPX file for trail {trailId}: {ex.Message}", ex);
        }
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
        catch (Exception)
        {
            return false;
        }
    }
    
    public async Task<Stream> DownloadGpxFileAsync(Guid trailId, string trailSlug)
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

            return new MemoryStream(bytes);
        }
        catch (Exception ex)
        {
            throw new FileNotFoundException($"Error accessing GPX file for trail {trailId}: {ex.Message}", ex);
        }
    }
}
