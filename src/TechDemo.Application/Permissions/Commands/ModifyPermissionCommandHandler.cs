using MediatR;
using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Permissions.Commands;

internal class ModifyPermissionsCommandHandler : IRequestHandler<ModifyPermissionsCommand, Result<Empty>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ModifyPermissionsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Empty>> Handle(ModifyPermissionsCommand request, CancellationToken cancellationToken)
    {
        var permissionResult = await _unitOfWork.PermissionsRepository.GetByIdAsync(request.Id, cancellationToken);
        var permissionTypeResult = PermissionType.FromDescription(request.PermissionType);

        return permissionResult
            .Map(permission =>
                permission.ModifyPermission(
                    request.EmployeeForename,
                    request.EmployeeSurname,
                    permissionTypeResult.IsSuccess ? permissionTypeResult.Value : default)
                .Project(_ => permission)
                .MapAsync(permission =>
                    _unitOfWork.PermissionsRepository.UpdateAsync(permission, cancellationToken)).Unwrap()
                .MapAsync(_ => _unitOfWork.SaveChangesAsync(cancellationToken)).Unwrap());
    }
}