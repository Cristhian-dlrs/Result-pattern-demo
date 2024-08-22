namespace TechDemo.Contracts.Permissions;

public record CreatePermissionRequest(
    string EmployeeForename,
    string EmployeeSurname,
    string PermissionType
);