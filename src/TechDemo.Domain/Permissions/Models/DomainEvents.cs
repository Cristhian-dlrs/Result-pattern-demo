using TechDemo.Domain.Shared.Models;

namespace TechDemo.Domain.Permissions.Models.Events;

public record PermissionRequestedEvent() : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Operation { get; } = "Request Permission";
}

public record PermissionModifiedEvent() : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Operation { get; } = "Modify Permission";
}