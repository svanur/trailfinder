using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrailFinder.Core.Interfaces.Repositories;
using TrailFinder.Infrastructure.Persistence;
using TrailFinder.Infrastructure.Persistence.Repositories;

namespace TrailFinder.Infrastructure;

// TrailFinder.Infrastructure/DependencyInjection.cs
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

        services.AddScoped<ITrailRepository, TrailRepository>();

        return services;
    }
}