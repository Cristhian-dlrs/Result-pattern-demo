namespace TechDemo.Api.Permissions.Requests;

public record CreatePermissionRequest(
    string EmployeeForename,
    string EmployeeSurname,
    string PermissionType);