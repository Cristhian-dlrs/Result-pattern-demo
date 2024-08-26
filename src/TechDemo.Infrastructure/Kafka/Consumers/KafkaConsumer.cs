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
    private readonly TaskCompletionSource<bool> _servicesInitializedSignal;
    private readonly IPermissionsViewRepository _permissionsViewRepository;

    public KafkaConsumer(
        IConsumer<Ignore, string> consumer,
        IOptions<KafkaOptions> options,
        IPermissionsViewRepository permissionsViewRepository,
        ILogger<KafkaConsumer> logger,
        TaskCompletionSource<bool> servicesInitializedSignal)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _permissionsViewRepository = permissionsViewRepository
            ?? throw new ArgumentNullException(nameof(permissionsViewRepository));
        _servicesInitializedSignal = servicesInitializedSignal
            ?? throw new ArgumentNullException(nameof(servicesInitializedSignal));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _servicesInitializedSignal.Task;
        _logger.LogInformation("Kafka consumer initialized, {@Date}", DateTime.UtcNow);

        try
        {
            _consumer.Subscribe(_options.DefaultTopic);

            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume();
                if (result == null) continue;

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
                        _logger.LogInformation(
                            "Event processed {@EventName}, {@Payload}, {@Date}",
                            requestEvent?.GetType().Name,
                            requestEvent,
                            DateTime.UtcNow);
                        break;

                    case nameof(Operations.Modify):
                        var modifyEvent = domainEvent as PermissionModifiedEvent;
                        var modifiedPermission = modifyEvent?.Permission;
                        await _permissionsViewRepository.UpdateAsync(modifiedPermission!, cancellationToken);
                        _logger.LogInformation(
                            "Event processed {@EventName}, {@Payload}, {@Date}",
                            modifyEvent?.GetType().Name,
                            modifyEvent,
                            DateTime.UtcNow);
                        break;

                    default:
                        throw new InvalidOperationException("Unable to process event.");
                }
            }
        }
        catch (ConsumeException ex)
        {
            _logger.LogError(
                "Error consuming events {@Error}, {@Date}",
                ex.Message,
                DateTime.UtcNow);
        }
        catch (TaskCanceledException)
        {
            _logger.LogError("Shutting down kafka consumer {@Date}", DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Unhandled error on kafka producer: {@Error}, {@Date}",
                ex.Message,
                DateTime.UtcNow);
        }
    }
}
