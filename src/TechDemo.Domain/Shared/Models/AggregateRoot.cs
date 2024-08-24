namespace TechDemo.Domain.Shared.Models;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => [.. _domainEvents];

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void FlushDomainEvents() => _domainEvents.Clear();
}