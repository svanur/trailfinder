using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using TrailFinder.Application.Services;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Core.Services.TrailAnalysis;
using TrailFinder.Core.ValueObjects;
using TrailFinder.Infrastructure.Configuration;
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

        // Repository dependency injection
        services.AddScoped<ITrailRepository, TrailRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IRaceRepository, RaceRepository>();
        services.AddScoped<IGpxFileRepository, GpxFileRepository>();
        services.AddScoped<IGpxService , GpxService>();
        
        // Add Supabase configuration
        services.Configure<SupabaseSettings>(settings =>
        {
            settings.Url = configuration["VITE_SUPABASE_URL"]!;
            settings.Key = configuration["VITE_SUPABASE_ANON_KEY"]!;
        });

        // Register services
        services.AddScoped<ISupabaseStorageService, SupabaseStorageService>();

        // Register GeometryFactory as a singleton since it's thread-safe
        services.AddSingleton<GeometryFactory>();

        //
        // Dependency injection
        //
        services.AddTransient<IAnalyzer<List<GpxPoint>, RouteType>, RouteAnalyzer>();
        services.AddTransient<IAnalyzer<TerrainAnalysisInput, TerrainType>, TerrainAnalyzer>();
        services.AddTransient<IAnalyzer<DifficultyAnalysisInput, DifficultyLevel>, DifficultyAnalyzer>();
        services.AddTransient<AnalysisService>();
        services.AddSingleton<GeometryFactory>(sp => NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
        
        return services;
    }
}