using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechDemo.Infrastructure.Producers;

namespace TechDemo.Infrastructure.Kafka;

public static class Extensions
{
    public static IServiceCollection AddKafkaWorkers(
        this IServiceCollection services)
    {
        services
            .AddOptions<KafkaOptions>()
            .BindConfiguration(nameof(KafkaOptions))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<KafkaOptions>>().Value;
            var kafkaConfig = new ProducerConfig
            {
                BootstrapServers = options.BootstrapServers,
                AllowAutoCreateTopics = options.AllowAutoCreateTopics,
                Acks = Acks.All,
            };

            return new ProducerBuilder<Null, string>(kafkaConfig).Build();
        });

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<KafkaOptions>>().Value;
            var kafkaConfig = new ConsumerConfig
            {
                BootstrapServers = options.BootstrapServers,
                GroupId = options.DefaultGroupId,
                AllowAutoCreateTopics = options.AllowAutoCreateTopics,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            return new ConsumerBuilder<Ignore, string>(kafkaConfig).Build();
        });

        services
            .AddHostedService<KafkaProducer>()
            .AddHostedService<KafkaConsumer>();

        return services;
    }
}