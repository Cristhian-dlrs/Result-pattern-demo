using FluentValidation;
using MediatR;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Permissions.Commands;

public record ModifyPermissionsCommand(
    Guid Id,
    string? EmployeeForename,
    string? EmployeeSurname,
    string? PermissionType) : IRequest<Result<None>>;

public class ModifyPermissionsCommandValidator : AbstractValidator<ModifyPermissionsCommand>
{
    public ModifyPermissionsCommandValidator()
    {
        RuleFor(x => x.Id)
          .NotNull();
    }
}