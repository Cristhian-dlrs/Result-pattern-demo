using MediatR;
using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Permissions.Commands;

public class RequestPermissionsCommandHandler : IRequestHandler<RequestPermissionsCommand, Result<None>>
{
    private readonly IUnitOfWork _unitOfWork;

    public RequestPermissionsCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<None>> Handle(RequestPermissionsCommand request, CancellationToken cancellationToken)
    {
        return await Permission.Create(
             request.EmployeeForename,
             request.EmployeeSurname,
             request.PermissionType)
         .ThenAsync(permission =>
             _unitOfWork.PermissionsRepository.CreateAsync(permission, cancellationToken)).Unwrap()
         .ThenAsync(_ => _unitOfWork.SaveChangesAsync(cancellationToken));
    }
}