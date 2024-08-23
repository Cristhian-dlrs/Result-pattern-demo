using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Models;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Domain.Shared.Results;
using TechDemo.Infrastructure.EntityFramework.Outbox;

namespace TechDemo.Infrastructure.EntityFramework.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext
            ?? throw new ArgumentNullException(nameof(dbContext));

        PermissionsRepository = new PermissionsRepository(dbContext);
    }

    public IPermissionsRepository PermissionsRepository { get; private set; }

    public async Task<Result<None>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        AddOutboxMessages();
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private void AddOutboxMessages()
    {
        var messages = MapDomainEventToOutboxMessages();
        _dbContext.AddRange(messages);
    }

    private IEnumerable<DeferredEvent> MapDomainEventToOutboxMessages()
        => _dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Any())
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                domainEvents
                    .ToList()
                    .ForEach(domainEvent => domainEvent.EntityId = entity.Id);
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent =>
                new DeferredEvent(
                    Guid.NewGuid(),
                    domainEvent.Operation,
                    JsonSerializer.Serialize(domainEvent),
                    DateTime.UtcNow,
                    null));

    public void Dispose() => _dbContext.Dispose();
}
