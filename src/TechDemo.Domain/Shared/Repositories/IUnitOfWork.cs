using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Domain.Shared.Repositories;

public interface IUnitOfWork : IDisposable
{
    public IPermissionsRepository PermissionsRepository { get; }
    public Task<Result<Empty>> SaveChangesAsync(CancellationToken cancellationToken);
}