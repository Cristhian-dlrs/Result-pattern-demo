using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Infrastructure.EntityFramework;
using TechDemo.Infrastructure.EntityFramework.Repositories;

public static class Extensions
{
    public static IServiceCollection AddEntityFramework(
        this IServiceCollection services, IConfiguration configuration)
    {

        services
            .AddOptions<SqlOptions>()
            .Bind(configuration.GetSection(nameof(SqlOptions)));

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            var sqlOptions = provider.GetRequiredService<SqlOptions>();
            options.UseSqlServer(sqlOptions.ConnectionString);
        });

        services.AddSingleton<IUnitOfWork, UnitOfWork>();

        return services;
    }
}