using TechDemo.Domain.Shared.Models;

namespace TechDemo.Domain.Permissions.Models.Events;

public record PermissionRequestedEvent(
        Guid id,
        string employeeForename,
        string employeeSurname,
        string permissionType,
        DateTime permissionDate) : IDomainEvent
{
    public string Operation { get; } = nameof(Operations.Request);
}

public record PermissionModifiedEvent(
        Guid id,
        string employeeForename,
        string employeeSurname,
        string permissionType,
        DateTime permissionDate) : IDomainEvent
{
    public string Operation { get; } = nameof(Operations.Modify);
}

public enum Operations
{
    Request,
    Modify
}