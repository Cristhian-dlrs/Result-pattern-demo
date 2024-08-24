using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TechDemo.Domain.Permissions.Models.Events;
using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Models;

namespace TechDemo.Infrastructure.Kafka;

internal class KafkaConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly KafkaOptions _options;
    ILogger<KafkaConsumer> _logger;
    private readonly IPermissionsViewRepository _permissionsViewRepository;

    public KafkaConsumer(
        IConsumer<Ignore, string> consumer,
        IOptions<KafkaOptions> options,
        IPermissionsViewRepository permissionsViewRepository,
        ILogger<KafkaConsumer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _permissionsViewRepository = permissionsViewRepository
            ?? throw new ArgumentNullException(nameof(permissionsViewRepository));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await Task.Delay(TimeSpan.FromSeconds(1));
        _logger.LogInformation("Kafka consumer initialized.");

        try
        {
            _consumer.Subscribe(_options.DefaultTopic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(TimeSpan.FromSeconds(5));
                if (result == null)
                {
                    continue;
                }

                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    result.Message.Value,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

                switch (domainEvent?.Operation)
                {
                    case nameof(Operations.Request):
                        var requestEvent = domainEvent as PermissionRequestedEvent;
                        var requestedPermission = requestEvent?.Permission;
                        await _permissionsViewRepository.AddAsync(requestedPermission!, cancellationToken);
                        break;

                    case nameof(Operations.Modify):
                        var modifyEvent = domainEvent as PermissionModifiedEvent;
                        var modifiedPermission = modifyEvent?.Permission;
                        await _permissionsViewRepository.UpdateAsync(modifiedPermission!, cancellationToken);
                        break;

                    default:
                        throw new InvalidOperationException("Unable to process event.");
                }

                _logger.LogInformation($"Event processed: {result.Message.Value}");
            }
        }
        catch (ConsumeException ex)
        {
            _logger.LogError(ex, "Error during message consumption.");
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("Kafka consumer was shutting down.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when processing events.");
        }
    }
}
