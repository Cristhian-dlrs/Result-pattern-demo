using FluentValidation;

namespace TechDemo.Contracts.Permissions;

public record CreatePermissionRequest(
    string EmployeeForename,
    string EmployeeSurname,
    string PermissionType)
{
    public const string RouteTemplate = "/permissions";
};

public class CreatePermissionRequestValidator : AbstractValidator<CreatePermissionRequest>
{
    public CreatePermissionRequestValidator()
    {
        RuleFor(x => x.EmployeeForename)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("EmployeeName is required.");

        RuleFor(x => x.EmployeeSurname)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("EmployeeSurname is required.");

        RuleFor(x => x.PermissionType)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("PermissionType is required");
    }
}