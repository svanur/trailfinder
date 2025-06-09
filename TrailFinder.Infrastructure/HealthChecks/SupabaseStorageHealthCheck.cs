using Microsoft.Extensions.Diagnostics.HealthChecks;
using Supabase;
namespace TrailFinder.Infrastructure.HealthChecks;
    
public class SupabaseStorageHealthCheck : IHealthCheck
{
    private readonly Client _supabaseClient;

    public SupabaseStorageHealthCheck(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _supabaseClient.Storage.ListBuckets();
            if (result == null)
            {
                return HealthCheckResult.Unhealthy("Cannot access Supabase Storage");
            }
            return HealthCheckResult.Healthy("Supabase Storage is accessible");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Supabase Storage check failed",
                ex);
        }
    }
}
