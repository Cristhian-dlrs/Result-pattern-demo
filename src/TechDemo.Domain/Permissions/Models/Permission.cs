using TechDemo.Domain.Permissions.Models.Events;
using TechDemo.Domain.Shared.Models;

namespace TechDemo.Domain.Permissions.Models;

public class Permission : AggregateRoot
{
    public string EmployeeForename { get; private set; }
    public string EmployeeLastName { get; private set; }
    public PermissionType PermissionType { get; private set; }
    public DateTime PermissionDate { get; }

    private Permission(string employeeForename, string employeeLastName, PermissionType permissionType)
    {
        EmployeeForename = employeeForename;
        EmployeeLastName = employeeLastName;
        PermissionType = permissionType;
        PermissionDate = DateTime.UtcNow;
        AddDomainEvent(new PermissionRequestedEvent());
    }

    public void ModifyEmployeeForename(string employeeForename)
    {
        EmployeeForename = employeeForename;
        AddDomainEvent(new PermissionModifiedEvent());
    }

    public void ModifyEmployeeLastName(string employeeLastName)
    {
        EmployeeLastName = employeeLastName;
        AddDomainEvent(new PermissionModifiedEvent());
    }

    public void ModifyEmployeeForename(PermissionType permissionType)
    {
        PermissionType = permissionType;
        AddDomainEvent(new PermissionModifiedEvent());
    }

    public static Permission Create(
        string employeeForename, string employeeLastName, PermissionType permissionType)
        => new Permission(employeeForename, employeeLastName, permissionType);
}