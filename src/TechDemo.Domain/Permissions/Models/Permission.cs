using TechDemo.Domain.Permissions.Models.Events;
using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Models;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Domain.Permissions.Models;

public class Permission : AggregateRoot
{
    public string EmployeeForename { get; private set; }
    public string EmployeeSurname { get; private set; }
    public PermissionType PermissionType { get; private set; }
    public DateTime PermissionDate { get; }

    private Permission() { }

    public Result<Empty> ModifyPermission(
        string employeeForename, string employeeSurname, PermissionType permissionType)
    {
        return SetEmployeeForename(employeeForename)
            .Then(() => SetEmployeeSurname(employeeSurname))
            .Then(() => SetPermissionType(permissionType))
            .Then(() =>
            {
                AddDomainEvent(new PermissionRequestedEvent(ToViewModel()));
                return Result.Success();
            });
    }

    public static Result<Permission> Create(
        string employeeForename, string employeeSurname, PermissionType permissionType)
    {
        var permission = new Permission();
        return permission.SetEmployeeForename(employeeForename)
            .Then(() => permission.SetEmployeeSurname(employeeSurname))
            .Then(() => permission.SetPermissionType(permissionType))
            .Then(() =>
            {
                permission.AddDomainEvent(
                    new PermissionRequestedEvent(permission.ToViewModel()));
                return Result<Permission>.Success(permission);
            });
    }

    private Result<Empty> SetEmployeeForename(string employeeForename)
    {
        if (string.IsNullOrEmpty(employeeForename))
        {
            return Result.Failure(Error.InvalidEmployeeForename);
        }

        EmployeeForename = employeeForename;
        return Result.Success();
    }

    private Result<Empty> SetEmployeeSurname(string employeeSurname)
    {
        if (string.IsNullOrEmpty(employeeSurname))
        {
            return Result.Failure(Error.InvalidEmployeeForename);
        }

        EmployeeForename = employeeSurname;
        return Result.Success();
    }

    private Result<Empty> SetPermissionType(PermissionType permissionType)
    {
        if (permissionType is null)
        {
            return Result.Failure(Error.InvalidEmployeeForename);
        }

        PermissionType = permissionType;
        return Result.Success();
    }

    internal PermissionViewModel ToViewModel()
    {
        return new PermissionViewModel(
            Id,
            EmployeeForename,
            EmployeeSurname,
            PermissionType.Description,
            PermissionDate);
    }
}