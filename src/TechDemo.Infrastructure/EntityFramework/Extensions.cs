using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

    public static async Task InitializeSqlDbAsync<T>(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromMinutes(5));

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<T>>();
            var dbContext = services.GetRequiredService<AppDbContext>();

            try
            {
                logger.LogInformation("Starting to apply database migrations...");

                await dbContext.Database.MigrateAsync(cancellationTokenSource.Token);

                logger.LogInformation("Database migrations applied successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while applying database migrations.");
                throw;
            }
        }
    }
}