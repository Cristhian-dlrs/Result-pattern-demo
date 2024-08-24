using TechDemo.Domain.Shared.Results;

namespace TechDemo.Domain.Permissions.Models;

public interface IPermissionsRepository
{
    public Task<Result<Permission>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<Result<Guid>> CreateAsync(Permission permission, CancellationToken cancellationToken);
    public Result<None> Update(Permission permission);
}