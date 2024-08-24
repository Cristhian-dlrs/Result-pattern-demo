using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Infrastructure.EntityFramework;
using TechDemo.Infrastructure.EntityFramework.Repositories;

public static class Extensions
{
    public static IServiceCollection AddEntityFramework(
        this IServiceCollection services)
    {
        services
            .AddOptions<SqlServerOptions>()
            .BindConfiguration(nameof(SqlServerOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            var sqlOptions = provider.GetRequiredService<IOptions<SqlServerOptions>>().Value;
            options.UseSqlServer(sqlOptions.ConnectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}