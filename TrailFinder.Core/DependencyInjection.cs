using Microsoft.Extensions.DependencyInjection;
using TrailFinder.Core.Interfaces.Services;
using TrailFinder.Core.Services;

namespace TrailFinder.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<ITrailService, TrailService>();
        // Add other services here

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        return services;
    }
}
