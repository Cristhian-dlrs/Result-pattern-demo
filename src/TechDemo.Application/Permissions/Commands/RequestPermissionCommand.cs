using FluentValidation;
using MediatR;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Permissions.Commands;

public record RequestPermissionsCommand(
    string EmployeeForename,
    string EmployeeSurname,
    string PermissionType) : IRequest<Result<Empty>>;


public class RequestPermissionsCommandValidator : AbstractValidator<RequestPermissionsCommand>
{
    public RequestPermissionsCommandValidator()
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