using Microsoft.EntityFrameworkCore;
using TechDemo.Domain.Permissions;
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

    public async Task<Result<None>> CreateAsync(Permission permission, CancellationToken cancellationToken)
    {
        await _dbContext.Permissions.AddAsync(permission, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<Permission>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(permission => permission.Id == id);

        return permission is null
            ? Result<Permission>.Failure(PermissionErrors.NotFound)
            : Result<Permission>.Success(permission);
    }

    public Result<None> Update(Permission permission)
    {
        _dbContext.Permissions.Update(permission);
        return Result.Success();
    }
}