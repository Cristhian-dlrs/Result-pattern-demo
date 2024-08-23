using MediatR;
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
        return await _unitOfWork.PermissionsRepository.GetByIdAsync(request.Id, cancellationToken).Unwrap()
            .MapAsync(permission =>
                    permission.ModifyPermission(
                        request.EmployeeForename,
                        request.EmployeeSurname,
                        request.PermissionType)
                    .Project(_ => permission)
                    .MapAsync(permission =>
                        _unitOfWork.PermissionsRepository.UpdateAsync(permission, cancellationToken)).Unwrap()
                    .MapAsync(_ => _unitOfWork.SaveChangesAsync(cancellationToken)));
    }
}