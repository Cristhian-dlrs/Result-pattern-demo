using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Infrastructure.EntityFramework;
using TechDemo.Infrastructure.EntityFramework.Repositories;

public static class Extensions
{
    public static IServiceCollection AddEntityFramework(
        this IServiceCollection services)
    {
        services
            .AddOptions<SqlOptions>()
            .BindConfiguration(nameof(SqlOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            var sqlOptions = provider.GetRequiredService<SqlOptions>();
            options.UseSqlServer(sqlOptions.ConnectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}