using TechDemo.Domain.Shared.Models;

namespace TechDemo.Infrastructure.Producers;

internal interface IKafkaProducer
{
    public Task PublishMessageAsync(IDomainEvent message, CancellationToken cancellationToken);
}