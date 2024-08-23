using TechDemo.Domain.Shared.Results;

namespace TechDemo.Domain.Permissions.ViewModels;

public interface IPermissionsViewRepository
{
    public Task<Result<IEnumerable<PermissionViewModel>>> GetAsync(
        string term, CancellationToken cancellationToken);

    public Task<Result<Empty>> AddAsync(
        PermissionViewModel permission, CancellationToken cancellationToken);

    public Task<Result<Empty>> UpdateAsync(
        PermissionViewModel permission, CancellationToken cancellationToken);
}