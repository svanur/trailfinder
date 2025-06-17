using Supabase.Storage;
using Supabase.Storage.Interfaces;

namespace TrailFinder.Core.Interfaces.Services;

public interface ISupabaseStorageService
{
    IStorageBucketApi<Bucket> Storage { get; }
    IStorageFileApi<FileObject> From(string bucket);
}
