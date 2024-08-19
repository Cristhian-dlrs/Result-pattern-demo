using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Infrastructure.EntityFramework;

namespace TechDemo.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        PermissionsRepository = new PermissionsRepository(dbContext);
    }

    public IPermissionsRepository PermissionsRepository { get; private set; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() => _dbContext.Dispose();
}
