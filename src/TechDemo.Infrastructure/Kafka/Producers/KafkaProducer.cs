using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechDemo.Infrastructure.EntityFramework;
using TechDemo.Infrastructure.EntityFramework.Outbox;
using TechDemo.Infrastructure.Kafka;

namespace TechDemo.Infrastructure.Producers;

internal class KafkaProducer : BackgroundService
{
    private readonly IProducer<Null, string> _producer;
    private readonly KafkaOptions _kafkaOptions;
    private readonly IServiceProvider _serviceProvider;
    ILogger<KafkaProducer> _logger;

    public KafkaProducer(
        IProducer<Null, string> producer,
        IOptions<KafkaOptions> kafkaOptions,
        ILogger<KafkaProducer> logger,
        IServiceProvider serviceProvider)
    {
        _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _kafkaOptions = kafkaOptions.Value ?? throw new ArgumentNullException(nameof(kafkaOptions));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogInformation("Starting event producer...");

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = _serviceProvider.GetService<AppDbContext>()
                ?? throw new ArgumentNullException(nameof(AppContext));

            var messages = await dbContext.Set<OutboxMessage>()
                .Where(message => message.ProcessedOn == null)
                .Take(_kafkaOptions.BatchSize)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
            {
                await PublishMessageAsync(message.Payload, cancellationToken);
                message.ProcessedOn = DateTime.UtcNow;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
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

        var message = new Message<Null, string> { Value = domainEvent };
        await _producer.ProduceAsync(_kafkaOptions.DefaultTopic, message, cancellationToken);
    }
}