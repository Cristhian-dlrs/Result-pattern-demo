using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Models;

namespace TechDemo.Domain.Permissions.Models.Events;

public record PermissionRequestedEvent(PermissionViewModel Permission) : IDomainEvent
{
    public string Operation { get; } = nameof(Operations.Request);
}

public record PermissionModifiedEvent(PermissionViewModel Permission) : IDomainEvent
{
    public string Operation { get; } = nameof(Operations.Modify);
}

public enum Operations
{
    Request,
    Modify
}