using TechDemo.Domain.Shared.Result;

namespace TechDemo.Domain.Permissions.Models;

public interface IPermissionsRepository
{
    public Task<Result<Permission>> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<Result> CreateAsync(Permission permission, CancellationToken cancellationToken);
    public Task<Result> UpdateAsync(Permission permission, CancellationToken cancellationToken);
}