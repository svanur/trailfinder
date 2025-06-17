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
}