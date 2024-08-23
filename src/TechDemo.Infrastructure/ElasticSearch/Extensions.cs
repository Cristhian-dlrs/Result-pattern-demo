using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using TechDemo.Domain.Permissions.ViewModels;

namespace TechDemo.Infrastructure.ElasticSearch;

public static class Extensions
{
    public static IServiceCollection AddElasticSearch(
        this IServiceCollection services)
    {
        services
            .AddOptions<ElasticSearchOptions>()
            .BindConfiguration(nameof(ElasticSearchOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(provider =>
        {
            var elasticSearchOptions = provider
                .GetRequiredService<IOptions<ElasticSearchOptions>>().Value;

            var settings = new ConnectionSettings(new Uri(elasticSearchOptions.Url))
            .PrettyJson()
            .DefaultIndex(elasticSearchOptions.DefaultIndex);

            var client = new ElasticClient(settings);

            client.Indices.Create(
                elasticSearchOptions.DefaultIndex,
                index => index.Map<PermissionViewModel>(permission => permission.AutoMap()));

            return client;
        });

        services.AddSingleton<IElasticClient>(provider =>
                    provider.GetRequiredService<ElasticClient>());

        services.AddSingleton<IPermissionsViewRepository, PermissionsViewRepository>();

        return services;
    }
}