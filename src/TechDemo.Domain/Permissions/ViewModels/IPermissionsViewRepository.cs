using TechDemo.Domain.Shared.Results;

namespace TechDemo.Domain.Permissions.ViewModels;

public interface IPermissionsViewRepository
{
    public Task<Result<IEnumerable<PermissionViewModel>>> GetAsync(
        string term, CancellationToken cancellationToken);

    public Task<Result<None>> AddAsync(
        PermissionViewModel permission, CancellationToken cancellationToken);

    public Task<Result<None>> UpdateAsync(
        PermissionViewModel permission, CancellationToken cancellationToken);
}