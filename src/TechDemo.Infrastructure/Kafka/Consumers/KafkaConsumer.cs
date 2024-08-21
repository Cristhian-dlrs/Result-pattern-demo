using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechDemo.Domain.Permissions.Models.Events;
using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Models;

namespace TechDemo.Infrastructure.Kafka;

internal class KafkaConsumer : BackgroundService
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly KafkaOptions _options;
    ILogger<KafkaConsumer> _logger;
    private readonly IPermissionsViewRepository _permissionsViewRepository;

    public KafkaConsumer(
        IConsumer<Null, string> consumer,
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

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting event consumer...");
        return Task.Run(() => Consume(cancellationToken), cancellationToken);
    }

    private void Consume(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_options.DefaultTopic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(cancellationToken);
                var domainEvent = JsonSerializer.Deserialize<IDomainEvent>(result.Message.Value);

                switch (domainEvent?.Operation)
                {
                    case nameof(Operations.Request):
                        var requestEvent = domainEvent as PermissionRequestedEvent;
                        var requestedPermission = requestEvent?.Permission;
                        _permissionsViewRepository.AddAsync(requestedPermission!, cancellationToken);
                        break;

                    case nameof(Operations.Modify):
                        var modifyEvent = domainEvent as PermissionModifiedEvent;
                        var modifiedPermission = modifyEvent?.Permission;
                        _permissionsViewRepository.UpdateAsync(modifiedPermission!, cancellationToken);
                        break;

                    default:
                        throw new InvalidOperationException("Cannot process this event.");
                }
            }
        }
        catch (ConsumeException ex)
        {
            _logger.LogError(ex, "Error producing message.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when processing events.");
        }
    }
}