using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
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

    public static void InitializeDb(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<AppDbContext>();
        var logger = services.GetRequiredService<ILogger<AppDbContext>>();

        logger.LogInformation("Checking for migrations to apply, {@Date}", DateTime.UtcNow);

        Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, attempt, context) =>
                {
                    logger.LogError(
                        "Fail to apply migrations, {@Error}, {@Date}",
                        exception.Message,
                        DateTime.UtcNow);
                })
            .Execute(() => dbContext.Database.Migrate());
    }
}