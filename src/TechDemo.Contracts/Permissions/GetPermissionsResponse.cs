namespace TechDemo.Contracts.Permissions;

public record GetPermissionsQueryResponse(
    bool IsSuccess,
    List<PermissionResult> Permissions,
    string? Error);