using MediatR;
using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Domain.Shared.Results;


namespace TechDemo.Application.Permissions.Commands;

internal class RequestPermissionsCommandHandler : IRequestHandler<RequestPermissionsCommand, Result<Empty>>
{
    private readonly IUnitOfWork _unitOfWork;

    public RequestPermissionsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Empty>> Handle(RequestPermissionsCommand request, CancellationToken cancellationToken)
    {
        return await PermissionType.FromDescription(request.PermissionType)
            .MatchAsync(
                onSuccess: async (permissionType) =>
                   await Permission.Create(
                        request.EmployeeForename,
                        request.EmployeeSurname,
                        permissionType)
                    .MapAsync(permission =>
                        _unitOfWork.PermissionsRepository.CreateAsync(permission, cancellationToken)).Resolve()
                    .MapAsync(_ => _unitOfWork.SaveChangesAsync(cancellationToken)),

                onFailure: permissionType => Task.FromResult(Result.Failure(permissionType))
            );
    }
}