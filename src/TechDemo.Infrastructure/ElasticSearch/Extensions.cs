using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using TechDemo.Domain.Permissions.ViewModels;

namespace TechDemo.Infrastructure.ElasticSearch;

public static class Extensions
{
    public static IServiceCollection AddElasticSearch(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<ElasticSearchOptions>()
            .Bind(configuration.GetSection(nameof(ElasticSearchOptions)));

        services.AddSingleton(provider =>
        {
            var elasticSearchOptions = provider.GetRequiredService<ElasticSearchOptions>();
            var settings = new ConnectionSettings(new Uri(elasticSearchOptions.Url))
            .PrettyJson()
            .DefaultIndex(elasticSearchOptions.DefaultIndex);

            var client = new ElasticClient(settings);

            client.Indices.Create(
                elasticSearchOptions.DefaultIndex,
                index => index.Map<PermissionViewModel>(permission => permission.AutoMap()));

            return client;
        });

        services.AddSingleton<IPermissionsViewRepository, PermissionsViewRepository>();

        return services;
    }
}