using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechDemo.Infrastructure.Producers;

namespace TechDemo.Infrastructure.Kafka;

public static class Extensions
{
    public static IServiceCollection AddKafka(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<KafkaOptions>()
            .Bind(configuration.GetSection(nameof(KafkaOptions)));

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<KafkaOptions>();
            var kafkaConfig = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
            };

            return new ProducerBuilder<Null, string>(kafkaConfig).Build();
        });

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<KafkaOptions>();
            var kafkaConfig = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
            };

            return new ConsumerBuilder<Null, string>(kafkaConfig).Build();
        });

        services
            .AddSingleton<IKafkaProducer, KafkaProducer>()
            .AddHostedService<KafkaConsumer>();

        return services;
    }
}