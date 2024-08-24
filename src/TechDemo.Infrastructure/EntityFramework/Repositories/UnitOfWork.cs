using Newtonsoft.Json;
using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Models;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Domain.Shared.Results;

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
        var deferredEvents = MapDomainEventsToDeferredEvents();
        _dbContext.DeferredEvents.AddRange(deferredEvents);
    }

    private IEnumerable<DeferredEvent> MapDomainEventsToDeferredEvents()
    {
        var deferredEvents = new List<DeferredEvent>();

        foreach (var entry in _dbContext.ChangeTracker.Entries<AggregateRoot>())
        {
            var entity = entry.Entity;

            if (entity.DomainEvents.Any())
            {
                var domainEvents = entity.DomainEvents.ToList();

                foreach (var domainEvent in domainEvents)
                {
                    domainEvent.EntityId = entity.Id;
                }

                entity.ClearDomainEvents();

                foreach (var domainEvent in domainEvents)
                {
                    var deferredEvent = new DeferredEvent(
                        Guid.NewGuid(),
                        domainEvent.Operation,
                        JsonConvert.SerializeObject(
                            domainEvent,
                            new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            }),
                        DateTime.UtcNow,
                        null
                    );

                    deferredEvents.Add(deferredEvent);
                }
            }
        }
        return deferredEvents;
    }
    // => _dbContext.ChangeTracker
    //     .Entries<AggregateRoot>()
    //     .Select(entry => entry.Entity)
    //     .Where(entity => entity.DomainEvents.Any())
    //     .SelectMany(entity =>
    //     {
    //         var domainEvents = entity.DomainEvents;
    //         domainEvents
    //             .ToList()
    //             .ForEach(domainEvent => domainEvent.EntityId = entity.Id);
    //         entity.ClearDomainEvents();
    //         return domainEvents;
    //     })
    //     .Select(domainEvent =>
    //         new DeferredEvent(
    //             Guid.NewGuid(),
    //             domainEvent.Operation,
    //             JsonSerializer.Serialize(domainEvent),
    //             DateTime.UtcNow,
    //             null));

    public void Dispose() => _dbContext.Dispose();
}
