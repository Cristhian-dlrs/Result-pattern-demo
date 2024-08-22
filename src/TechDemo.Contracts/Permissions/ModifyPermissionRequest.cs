namespace TechDemo.Contracts.Permissions;

public record ModifyPermissionRequest(
    int Id,
    string? EmployeeForename,
    string? EmployeeSurname,
    string? PermissionType
);