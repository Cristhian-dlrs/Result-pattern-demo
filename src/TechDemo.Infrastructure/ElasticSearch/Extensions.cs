using FluentValidation.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using Polly;
using TechDemo.Domain.Permissions.ViewModels;
using Policy = Polly.Policy;

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

            Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)))
                .Execute(() =>
                {
                    var result = client.Indices.Create(
                            elasticSearchOptions.DefaultIndex,
                            index => index.Map<PermissionViewModel>(permission => permission.AutoMap()));

                    if (result.ServerError.Error.Type != ElasticSearchErrors.IndexAlreadyCreated.Code)
                    {
                        throw new Exception("Error initializing elastic search");
                    }
                });

            return client;
        });

        services.AddSingleton<IElasticClient>(provider =>
                    provider.GetRequiredService<ElasticClient>());

        services.AddSingleton<IPermissionsViewRepository, PermissionsViewRepository>();

        return services;
    }
}