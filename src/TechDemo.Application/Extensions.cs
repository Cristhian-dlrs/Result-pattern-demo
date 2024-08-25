using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TechDemo.Application;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
                    configuration.RegisterServicesFromAssembly(typeof(ApplicationExtensions).Assembly));

        return services;
    }
}