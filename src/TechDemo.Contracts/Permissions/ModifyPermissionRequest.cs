using FluentValidation;

namespace TechDemo.Contracts.Permissions;

public record ModifyPermissionRequest(
    int Id,
    string? EmployeeForename,
    string? EmployeeSurname,
    string? PermissionType)
{
    public const string RouteTemplate = "/permissions/{permissionId:int}";
    public static string BuildRoute(int permissionId)
        => RouteTemplate.Replace("{permissionId:int}", permissionId.ToString());
}


public class ModifyPermissionRequestValidator : AbstractValidator<ModifyPermissionRequest>
{
    public ModifyPermissionRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Permission id is required.");
    }
}