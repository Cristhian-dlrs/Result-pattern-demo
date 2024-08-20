
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using TechDemo.Domain.Shared.Models;
using TechDemo.Infrastructure.Kafka;

namespace TechDemo.Infrastructure.Producers;

internal class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaOptions _kafkaOptions;

    public KafkaProducer(
        IProducer<Null, string> producer, IOptions<KafkaOptions> kafkaOptions)
    {
        _producer = producer
            ?? throw new ArgumentNullException(nameof(producer));

        _kafkaOptions = kafkaOptions.Value
            ?? throw new ArgumentNullException(nameof(kafkaOptions));
    }

    public async Task PublishMessageAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var domainEventJson = JsonSerializer.Serialize(domainEvent);
        var message = new Message<Null, string> { Value = domainEventJson };

        await _producer.ProduceAsync(_kafkaOptions.DefaultTopic, message, cancellationToken);
    }
}