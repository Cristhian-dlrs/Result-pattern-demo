using TechDemo.Domain.Shared.Results;

namespace TechDemo.Domain.Permissions.Models;

public interface IPermissionsRepository
{
    public Task<Result<Permission>> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<Result> CreateAsync(Permission permission, CancellationToken cancellationToken);
    public Task<Result> UpdateAsync(Permission permission, CancellationToken cancellationToken);
}