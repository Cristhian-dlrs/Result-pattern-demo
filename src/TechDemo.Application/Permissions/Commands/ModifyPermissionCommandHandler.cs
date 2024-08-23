using MediatR;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Permissions.Commands;

internal class ModifyPermissionsCommandHandler : IRequestHandler<ModifyPermissionsCommand, Result<None>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ModifyPermissionsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<None>> Handle(ModifyPermissionsCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.PermissionsRepository.GetByIdAsync(request.Id, cancellationToken).Unwrap()
            .ThenAsync(permission =>
                    permission.ModifyPermission(
                        request.EmployeeForename,
                        request.EmployeeSurname,
                        request.PermissionType)
                    .Map(_ => permission)
                    .ThenAsync(permission =>
                        _unitOfWork.PermissionsRepository.UpdateAsync(permission, cancellationToken)).Unwrap()
                    .ThenAsync(_ => _unitOfWork.SaveChangesAsync(cancellationToken)));
    }
}