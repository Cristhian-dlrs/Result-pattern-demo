using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Models;

namespace TechDemo.Domain.Permissions.Models.Events;

public record PermissionRequestedEvent(PermissionViewModel Permission) : IDomainEvent
{
    public string Operation { get; } = "Request Permission";
}

public record PermissionModifiedEvent(PermissionViewModel Permission) : IDomainEvent
{
    public string Operation { get; } = "Modify Permission";
}