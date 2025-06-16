using Supabase.Storage;
using Supabase.Storage.Interfaces;
using TrailFinder.Core.DTOs.Trails;

namespace TrailFinder.Core.Interfaces.Services;

public interface ISupabaseService
{
    IStorageBucketApi<Bucket> Storage { get; }
    
    IStorageFileApi<FileObject> From(string bucket);

    Task<TrailDto?> GetTrailBySlugAsync(string slug, CancellationToken cancellationToken = default);
    
    // Add other methods as needed
    Task<TrailDto?> GetTrailByIdAsync(Guid trailId, CancellationToken cancellationToken = default);
}