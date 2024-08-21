using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TechDemo.Domain.Permissions.ViewModels;

namespace TechDemo.Infrastructure.Kafka;

internal class KafkaConsumer : BackgroundService
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly KafkaOptions _options;
    private readonly IPermissionsViewRepository _permissionsViewRepository;

    public KafkaConsumer(
        IConsumer<Null, string> consumer,
        IOptions<KafkaOptions> options,
        IPermissionsViewRepository permissionsViewRepository)
    {
        _consumer = consumer
            ?? throw new ArgumentNullException(nameof(consumer));

        _options = options.Value
            ?? throw new ArgumentNullException(nameof(options));

        _permissionsViewRepository = permissionsViewRepository
            ?? throw new ArgumentNullException(nameof(permissionsViewRepository));
    }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => Consume(cancellationToken), cancellationToken);
    }

    private void Consume(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_options.DefaultTopic);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

            }
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation gracefully
        }
        finally
        {
            _consumer.Close();
        }
    }
}