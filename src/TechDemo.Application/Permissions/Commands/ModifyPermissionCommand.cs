using FluentValidation;
using MediatR;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Permissions.Commands;

public record ModifyPermissionsCommand(
    int Id,
    string EmployeeForename,
    string EmployeeSurname,
    string PermissionType) : IRequest<Result<Empty>>;

public class ModifyPermissionsCommandValidator : AbstractValidator<ModifyPermissionsCommand>
{
    public ModifyPermissionsCommandValidator()
    {
        RuleFor(x => x.Id)
          .GreaterThan(0);
    }
}