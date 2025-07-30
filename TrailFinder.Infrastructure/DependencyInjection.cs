using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using TrailFinder.Contract.Persistence;
using TrailFinder.Core.DTOs.GpxFile;
using TrailFinder.Core.Enums;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Core.Services.TrailAnalysis;
using TrailFinder.Core.Services.TrailAnalysis.DifficultyAnalysis;
using TrailFinder.Core.Services.TrailAnalysis.RouteAnalysis;
using TrailFinder.Core.Services.TrailAnalysis.TerrainAnalysis;
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
        services.AddScoped<IGpxService, GpxService>();

        // Register Analyzers
        // 1. RouteAnalyzer
        // This specific IAnalyzer implementation
        services.AddTransient<IAnalyzer<List<GpxPoint>, RouteType>, RouteAnalyzer>();

        // 2. TerrainAnalyzer
        // This specific IAnalyzer implementation
        services.AddTransient<IAnalyzer<TerrainAnalysisInput, TerrainType>, TerrainAnalyzer>();

        // 3. DifficultyAnalyzers and their Factory
        services.AddTransient<PavedRouteDifficultyAnalyzer>();
        services.AddTransient<TrailRouteDifficultyAnalyzer>();
        services.AddTransient<DifficultyAnalyzer>(); // The original/fallback analyzer
        services.AddTransient<DifficultyAnalyzerFactory>();

        // 4. AnalysisService (now with all its IAnalyzer dependencies resolved)
        services.AddTransient<IAnalysisService, AnalysisService>(); // Assuming you made IAnalysisService

        // 5. GpxService (now uses IGpxService and its dependencies are resolved)
        services
            .AddScoped<IGpxService,
                GpxService>(); // Use Scoped for services that might use DbContext/HttpClients per request

        // 6. IOsmLookupService (if it's a real implementation, register it)
        // If you mocked it in your tests, ensure your *actual* application has a real implementation registered.
        // Example:
        // services.AddTransient<IOsmLookupService, OsmLookupService>();
        // Or if you only use it within GpxService and it's a simple mock for now,
        // ensure it's handled. For a real app, you need a concrete implementation.
        // If you don't have a real IOsmLookupService yet, you can temporarily
        // register a dummy one to get DI working, but eventually it needs implementation.
        // For example, a mock for development:
        // services.AddSingleton<IOsmLookupService>(new Mock<IOsmLookupService>().Object); // Only for dev/testing, not production!


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
        services.AddTransient<PavedRouteDifficultyAnalyzer>();
        services.AddTransient<TrailRouteDifficultyAnalyzer>();
        services.AddTransient<DifficultyAnalyzer>(); // A difficulty analyzer fallback
        services.AddTransient<DifficultyAnalyzerFactory>(); // Register the factory
        services.AddTransient<AnalysisService>();
        services.AddTransient<IOsmLookupService, OsmLookupService>();

        services.AddSingleton<GeometryFactory>(sp => NtsGeometryServices.Instance.CreateGeometryFactory(4326));

        return services;
    }
}