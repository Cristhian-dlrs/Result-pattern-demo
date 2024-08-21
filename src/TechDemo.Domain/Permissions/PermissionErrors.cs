using TechDemo.Domain.Shared.Results;

namespace TechDemo.Domain.Permissions;

public sealed record PermissionErrors(string Code, string Description)
{
    public static readonly Error InvalidEmployeeForename = new(
        "Permissions.InvalidEmployeeForename",
        "The employee forename is required and cannot be null or empty.");

    public static readonly Error InvalidEmployeeSurname = new(
        "Permissions.InvalidEmployeeSurname",
        "The employee surname is required and cannot be null or empty.");

    public static readonly Error InvalidPermissionType = new(
        "Permissions.InvalidPermissionType",
        "The permission type is required and cannot be null.");

    public static readonly Error InvalidPermissionDescription = new(
        "Permissions.InvalidPermissionType",
        "The provided description does not belong to any registered permission type.");

    public static readonly Error InvalidPermissionId = new(
        "Permissions.InvalidPermissionId",
        "The provided id does not belong to any registered permission type.");
}