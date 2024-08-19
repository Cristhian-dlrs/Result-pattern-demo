using TechDemo.Domain.Shared.Result;

namespace TechDemo.Domain.Permissions.ViewModels;

public interface IPermissionsViewRepository
{
    public Task<Result<IEnumerable<PermissionViewModel>>> GetAsync(
        string term, CancellationToken cancellationToken);
}