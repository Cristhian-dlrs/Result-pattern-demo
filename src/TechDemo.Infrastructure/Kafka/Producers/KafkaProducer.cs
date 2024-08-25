using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechDemo.Infrastructure.EntityFramework;
using TechDemo.Infrastructure.Kafka;

namespace TechDemo.Infrastructure.Producers;

internal class KafkaProducer : BackgroundService
{
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaOptions _kafkaOptions;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly TaskCompletionSource<bool> _servicesInitializedSignal;


    public KafkaProducer(
        IProducer<Null, string> producer,
        IOptions<KafkaOptions> kafkaOptions,
        ILogger<KafkaProducer> logger,
        IServiceScopeFactory serviceScopeFactory,
        TaskCompletionSource<bool> servicesInitializedSignal)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _kafkaOptions = kafkaOptions.Value ?? throw new ArgumentNullException(nameof(kafkaOptions));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _servicesInitializedSignal = servicesInitializedSignal
            ?? throw new ArgumentNullException(nameof(servicesInitializedSignal));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _servicesInitializedSignal.Task;
        _logger.LogInformation("Kafka producer initialized, {@Date}", DateTime.UtcNow);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var messages = await dbContext.Set<DeferredEvent>()
                    .Where(@event => @event.ProcessedOn == null)
                    .OrderBy(@event => @event.RegisteredOn)
                    .Take(_kafkaOptions.BatchSize)
                    .ToListAsync(cancellationToken);

                foreach (var message in messages)
                {
                    await PublishMessageAsync(message.Payload, cancellationToken);
                    message.ProcessedOn = DateTime.UtcNow;
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        catch (ProduceException<Null, string> ex)
        {
            _logger.LogError(
                "Error producing events {@Error}, {@Date}",
                ex.Message,
                DateTime.UtcNow);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(
                "Error updating deferred events {@Error}, {@Date}",
                ex.Message,
                DateTime.UtcNow);
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("Shutting down kafka producer, {@Date}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Unhandled error on kafka producer: {@Error}, {@Date}",
                ex.Message,
                DateTime.UtcNow);
        }
    }

    private async Task PublishMessageAsync(string domainEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var @event = new Message<Null, string> { Value = domainEvent };
        var producerResult = await _producer.ProduceAsync(_kafkaOptions.DefaultTopic, @event, cancellationToken);
        _logger.LogInformation("Event published {@Payload}, {@Offset},  {@Date}",
            producerResult.Message.Value,
            producerResult.Offset,
            DateTime.UtcNow);
    }
}