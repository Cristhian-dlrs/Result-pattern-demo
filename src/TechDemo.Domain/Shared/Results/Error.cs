namespace TechDemo.Domain.Shared.Results;

public sealed record Error(string Code, string Description)
{
    // Generic errors
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Null Value", "The value cannot be null.");

    // Permissions errors
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

    // View repository errors
    public static readonly Error AddViewError = new(
        "ViewRepository.AddViewError",
        "Unable to add the view model.");

    public static readonly Error UpdateViewError = new(
        "ViewRepository.UpdateViewError",
        "Unable to update vew model.");

    public static readonly Error QueryError = new(
        "ViewRepository.QueryError",
        "Unable to perform the query.");
}