namespace TechDemo.Contracts.Permissions;

public record PermissionResult(
    int Id,
    string EmployeeForename,
    string EmployeeSurname,
    string PermissionType,
    DateTime CreatedDate
);