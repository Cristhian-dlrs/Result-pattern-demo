using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Models;

namespace TechDemo.Domain.Permissions.Models.Events;

public record PermissionRequestedEvent(PermissionViewModel Permission) : IDomainEvent
{
    public string Operation { get; } = nameof(Operations.Request);
    public int EntityId { get; set; }
}

public record PermissionModifiedEvent(PermissionViewModel Permission) : IDomainEvent
{
    public string Operation { get; } = nameof(Operations.Modify);
    public int EntityId { get; set; }
}

public enum Operations
{
    Request,
    Modify
}