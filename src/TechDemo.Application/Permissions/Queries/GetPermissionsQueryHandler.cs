using MediatR;
using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Permissions.Queries;

internal class GetPermissionsQueryHandler
    : IRequestHandler<GetPermissionsQuery, Result<GetPermissionsQueryResult>>
{
    private readonly IPermissionsViewRepository _permissionsViewRepository;

    public GetPermissionsQueryHandler(IPermissionsViewRepository permissionsRepository)
    {
        _permissionsViewRepository = permissionsRepository;
    }

    public async Task<Result<GetPermissionsQueryResult>> Handle(
        GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await _permissionsViewRepository
            .GetAsync(request.SearchTerm ?? string.Empty, cancellationToken).Unwrap()
            .ThenAsync(permissions =>
                    Result<GetPermissionsQueryResult>.Success(
                         new GetPermissionsQueryResult(permissions)).Async());
    }
}