using TechDemo.Domain.Shared.Results;

namespace TechDemo.Domain.Permissions.Models;

public interface IPermissionsRepository
{
    public Task<Result<Permission>> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<Result<Empty>> CreateAsync(Permission permission, CancellationToken cancellationToken);
    public Task<Result<Empty>> UpdateAsync(Permission permission, CancellationToken cancellationToken);
}