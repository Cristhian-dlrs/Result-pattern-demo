namespace TechDemo.Domain.Permissions.ViewModels;

public interface IPermissionsViewRepository
{
    public Task<IEnumerable<PermissionViewModel>?> GetAsync(
        string term, int resultNumber, CancellationToken cancellationToken);

    public Task<bool> InsertAsync(
        PermissionViewModel permission, CancellationToken cancellationToken);

    public Task<bool> UpdateAsync(
        PermissionViewModel permission, CancellationToken cancellationToken);
}