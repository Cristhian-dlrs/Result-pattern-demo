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
    ILogger<KafkaProducer> _logger;

    public KafkaProducer(
        IProducer<Null, string> producer,
        IOptions<KafkaOptions> kafkaOptions,
        ILogger<KafkaProducer> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
        _kafkaOptions = kafkaOptions.Value ?? throw new ArgumentNullException(nameof(kafkaOptions));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var messages = await dbContext.Set<DeferredEvent>()
                    .Where(message => message.ProcessedOn == null)
                    .Take(_kafkaOptions.BatchSize)
                    .OrderBy(message => message.OcurredOn)
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
            _logger.LogError(ex, "Error producing message.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error saving changes to the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when publishing events.");
        }
    }

    private async Task PublishMessageAsync(string domainEvent, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var @event = new Message<Null, string> { Value = domainEvent };
        var producerResult = await _producer.ProduceAsync(_kafkaOptions.DefaultTopic, @event, cancellationToken);
        _logger.LogInformation($"Event published {producerResult.Value}, Offset: {producerResult.Offset}");
    }
}