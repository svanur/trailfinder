using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using TrailFinder.Application.Common;
using TrailFinder.Application.Common.Behaviors;

namespace TrailFinder.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(IApplicationMarker).Assembly;

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(assembly);
            // Add pipeline behaviors
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Register AutoMapper
        services.AddAutoMapper(assembly);

        // Register FluentValidation
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>();

        return services;
    }
}