using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Result;
using TechDemo.Infrastructure.EntityFramework;

namespace TechDemo.Infrastructure.Repositories;

internal class PermissionsRepository : IPermissionsRepository
{
    private readonly AppDbContext _dbContext;

    public PermissionsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Result> CreateAsync(Permission permission, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Permission>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateAsync(Permission permission, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
