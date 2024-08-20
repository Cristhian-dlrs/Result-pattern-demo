using TechDemo.Domain.Permissions.Models;
using TechDemo.Domain.Shared.Models;
using TechDemo.Domain.Shared.Repositories;
using TechDemo.Infrastructure.Producers;

namespace TechDemo.Infrastructure.EntityFramework.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private readonly IKafkaProducer _producer;

    public UnitOfWork(AppDbContext dbContext, IKafkaProducer producer)
    {
        _dbContext = dbContext
            ?? throw new ArgumentNullException(nameof(dbContext));

        _producer = producer
            ?? throw new ArgumentException(nameof(producer));

        PermissionsRepository = new PermissionsRepository(dbContext);
    }

    public IPermissionsRepository PermissionsRepository { get; private set; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var domainEvents = GetDomainEvents();

        // TODO: outbox pattern here!
        foreach (var domainEvent in domainEvents)
        {
            await _producer.PublishMessageAsync(domainEvent, cancellationToken);
        }

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private IEnumerable<IDomainEvent> GetDomainEvents()
        => _dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Any())
            .SelectMany(entity =>
            {
                var domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            });

    public void Dispose() => _dbContext.Dispose();
}
