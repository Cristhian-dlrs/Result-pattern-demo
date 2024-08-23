using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechDemo.Infrastructure.ElasticSearch;
using TechDemo.Infrastructure.Kafka;

namespace TechDemo.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddKafkaWorkers();
        services.AddElasticSearch();
        services.AddEntityFramework(configuration);
        return services;
    }
}