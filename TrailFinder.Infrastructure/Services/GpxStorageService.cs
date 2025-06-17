using TrailFinder.Core.Exceptions;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;

namespace TrailFinder.Infrastructure.Services;

public class SupabaseGpxStorageService : IGpxStorageService
{
    private readonly ISupabaseStorageService _storageService;
    private readonly ITrailRepository _trailRepository;
    private const string BucketName = "gpx-files";

    public SupabaseGpxStorageService(
        ISupabaseStorageService storageService,
        ITrailRepository trailRepository)
    {
        _storageService = storageService;
        _trailRepository = trailRepository;
    }

    public async Task<bool> UploadGpxFileAsync(Guid trailId, Stream fileStream, string fileName)
    {
        var trail = await _trailRepository.GetByIdAsync(trailId);
        if (trail == null)
        {
            throw new TrailNotFoundException($"Trail not found with ID {trailId}");
        }
        
        var filePath = $"{trail.Slug}/{trailId}/{fileName}";
        
        try 
        {
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var uploadResult = await _storageService
                .From(BucketName)
                .Upload(fileBytes, filePath);

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
        var trail = await _trailRepository.GetByIdAsync(trailId);
        if (trail == null)
        {
            throw new TrailNotFoundException($"Trail not found with ID {trailId}");
        }

        var filePath = $"{trail.Slug}/{trailId}.gpx";
        try
        {
            var bytes = await _storageService
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
