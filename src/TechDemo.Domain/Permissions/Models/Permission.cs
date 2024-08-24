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

    private Permission() { }

    public Result<None> ModifyPermission(
        string? employeeForename, string? employeeSurname, string? permissionType)
    {
        return Result.Success()
            .Then(_ => employeeForename is null
                ? Result.Success()
                : SetEmployeeForename(employeeForename))
            .Then(_ => employeeSurname is null
                ? Result.Success()
                : SetEmployeeSurname(employeeSurname))
            .Then(_ => permissionType is null
                ? Result.Success()
                : SetPermissionType(permissionType))
            .Then(_ =>
            {
                AddDomainEvent(new PermissionRequestedEvent(
                    Id,
                    EmployeeForename,
                    EmployeeSurname,
                    PermissionType.Description,
                    PermissionDate));
                return Result.Success();
            });
    }

    public static Result<Permission> Create(
        string employeeForename, string employeeSurname, string permissionType)
    {
        var permission = new Permission { Id = Guid.NewGuid() };
        return permission.SetEmployeeForename(employeeForename)
            .Then(_ => permission.SetEmployeeSurname(employeeSurname))
            .Then(_ => permission.SetPermissionType(permissionType))
            .Then(_ =>
            {
                permission.AddDomainEvent(new PermissionRequestedEvent(
                    permission.Id,
                    permission.EmployeeForename,
                    permission.EmployeeSurname,
                    permission.PermissionType.Description,
                    permission.PermissionDate));
                return Result<Permission>.Success(permission);
            });
    }

    private Result<None> SetEmployeeForename(string employeeForename)
    {
        if (string.IsNullOrEmpty(employeeForename))
        {
            return Result.Failure(PermissionErrors.InvalidEmployeeForename);
        }

        EmployeeForename = employeeForename;
        return Result.Success();
    }

    private Result<None> SetEmployeeSurname(string employeeSurname)
    {
        if (string.IsNullOrEmpty(employeeSurname))
        {
            return Result.Failure(PermissionErrors.InvalidEmployeeForename);
        }

        EmployeeSurname = employeeSurname;
        return Result.Success();
    }

    private Result<None> SetPermissionType(string permissionType)
    {
        return PermissionType.FromDescription(permissionType)
            .Then(permissionType =>
            {
                PermissionType = permissionType;
                return Result.Success();
            });
    }
}