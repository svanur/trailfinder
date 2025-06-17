using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrailFinder.Application.Services;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Infrastructure.Configuration;
using TrailFinder.Infrastructure.Mapping;
using TrailFinder.Infrastructure.Persistence;
using TrailFinder.Infrastructure.Persistence.Repositories;
using TrailFinder.Infrastructure.Services;

namespace TrailFinder.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                x => x.UseNetTopologySuite()
            ));

        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ITrailRepository, TrailRepository>();
        
        // Add AutoMapper configuration
        services.AddAutoMapper(typeof(SupabaseMappingProfile).Assembly);

        // Add Supabase configuration
        services.Configure<SupabaseSettings>(settings =>
        {
            settings.Url = configuration["VITE_SUPABASE_URL"]!;
            settings.Key = configuration["VITE_SUPABASE_ANON_KEY"]!;
        });

        // Register Supabase service
        services.AddScoped<ISupabaseStorageService, SupabaseStorageService>();

        // Add other repositories here
        services.AddScoped<IGpxService, GpxService>();
        services.AddScoped<IGpxStorageService, SupabaseGpxStorageService>();
        
        return services;
    }
}
