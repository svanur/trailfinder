using Microsoft.Extensions.DependencyInjection;

namespace TrailFinder.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        // Add other services here

        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        return services;
    }
}
