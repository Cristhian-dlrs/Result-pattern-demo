using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Infrastructure.EntityFramework.Repositories;

internal class PermissionsRepository : IPermissionsRepository
{
    private readonly AppDbContext _dbContext;

    public PermissionsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<Result<Empty>> CreateAsync(Permission permission, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Permission>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Empty>> UpdateAsync(Permission permission, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
