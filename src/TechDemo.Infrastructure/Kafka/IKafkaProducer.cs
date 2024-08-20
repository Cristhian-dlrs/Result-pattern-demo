using TechDemo.Domain.Shared.Models;

namespace TechDemo.Infrastructure.Kafka;

internal interface IKafkaProducer
{
    public Task PublishMessageAsync(IDomainEvent message, CancellationToken cancellationToken);
}