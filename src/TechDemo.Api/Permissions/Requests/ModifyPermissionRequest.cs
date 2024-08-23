namespace TechDemo.Api.Permissions.Requests;

public record ModifyPermissionRequest(
    string EmployeeForename,
    string EmployeeSurname,
    string PermissionType);