using TechDemo.Domain.Permissions.Models.Events;
using TechDemo.Domain.Shared.Models;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Domain.Permissions.Models;

public class Permission : AggregateRoot
{
    public string EmployeeForename { get; private set; }
    public string EmployeeSurname { get; private set; }
    public PermissionType PermissionType { get; private set; }
    public DateTime PermissionDate { get; }

    private Permission(string employeeForename, string employeeSurname, PermissionType permissionType)
    {
        EmployeeForename = employeeForename;
        EmployeeSurname = employeeSurname;
        PermissionType = permissionType;
        PermissionDate = DateTime.UtcNow;
        AddDomainEvent(new PermissionRequestedEvent());
    }

    public Result ModifyEmployeeForename(string employeeForename)
    {
        if (string.IsNullOrEmpty(employeeForename))
        {
            return Result.Failure(Error.InvalidEmployeeForename);
        }

        EmployeeForename = employeeForename;
        AddDomainEvent(new PermissionModifiedEvent());
        return Result.Success(true);
    }

    public Result ModifyEmployeeLastName(string employeeSurname)
    {
        if (string.IsNullOrEmpty(employeeSurname))
        {
            return Result.Failure(Error.InvalidEmployeeForename);
        }

        EmployeeSurname = employeeSurname;
        AddDomainEvent(new PermissionModifiedEvent());
        return Result.Success(true);
    }

    public Result ModifyEmployeeForename(PermissionType permissionType)
    {
        if (permissionType is null)
        {
            return Result.Failure(Error.InvalidPermissionType);
        }

        PermissionType = permissionType;
        AddDomainEvent(new PermissionModifiedEvent());
        return Result.Success(true);
    }

    public static Result<Permission> Create(
        string employeeForename, string employeeSurname, PermissionType permissionType)
    {
        if (string.IsNullOrEmpty(employeeForename))
        {
            return Result.Failure<Permission>(Error.InvalidEmployeeForename);
        }

        if (string.IsNullOrEmpty(employeeSurname))
        {
            return Result.Failure<Permission>(Error.InvalidEmployeeSurname);
        }

        if (permissionType is null)
        {
            return Result.Failure<Permission>(Error.InvalidPermissionType);
        }

        return new Permission(employeeForename, employeeSurname, permissionType);
    }
}