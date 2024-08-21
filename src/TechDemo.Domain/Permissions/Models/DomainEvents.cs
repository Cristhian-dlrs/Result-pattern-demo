using TechDemo.Domain.Shared.Models;

namespace TechDemo.Domain.Permissions.Models.Events;

public record PermissionRequestedEvent(Permission Permission) : IDomainEvent
{
    public Guid CorrelationId { get; } = Guid.NewGuid();
    public string Operation { get; } = "Request Permission";
}

public record PermissionModifiedEvent(Permission Permission) : IDomainEvent
{
    public Guid CorrelationId { get; } = Guid.NewGuid();
    public string Operation { get; } = "Modify Permission";
}