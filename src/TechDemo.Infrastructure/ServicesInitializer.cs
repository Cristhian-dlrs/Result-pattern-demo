using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using TechDemo.Infrastructure.EntityFramework;
using Policy = Polly.Policy;

namespace TechDemo.Infrastructure.Kafka;

public class ServicesInitializer : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ServicesInitializer> _logger;
    private readonly TaskCompletionSource<bool> _taskCompletionSignal;

    public ServicesInitializer(
        IServiceScopeFactory serviceScopeFactory, ILogger<ServicesInitializer> logger, TaskCompletionSource<bool> taskCompletionSignal)
    {
        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _taskCompletionSignal = taskCompletionSignal
            ?? throw new ArgumentNullException(nameof(taskCompletionSignal));
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Initializing Services...");
        await InitializeKafkaAsync();
        await InitializeSqlDbAsync();
        _taskCompletionSignal.SetResult(true);
    }

    private async Task InitializeSqlDbAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<AppDbContext>();

        _logger.LogInformation("Starting to apply database migrations...");
        await Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, attempt, context) =>
                {
                    _logger.LogError(exception, "Retrying database migration due to transient failure...");
                })
            .Execute(() => dbContext.Database.MigrateAsync());
    }

    private async Task InitializeKafkaAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var services = scope.ServiceProvider;
        var adminClient = services.GetRequiredService<IAdminClient>();
        var options = services.GetRequiredService<IOptions<KafkaOptions>>().Value;

        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, attempt, context) =>
                {
                    _logger.LogError(exception, "Retrying Kafka topic initialization due to transient failure...");
                })
            .ExecuteAsync(async () =>
            {
                var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));

                var topicSpecification = new TopicSpecification
                {
                    Name = options.DefaultTopic,
                    NumPartitions = options.PartitionsNumber,
                    ReplicationFactor = options.ReplicationFactor
                };

                if (!metadata.Topics.Any(topic => topic.Topic == options.DefaultTopic))
                {
                    _logger.LogInformation("Initializing default topic...");

                    await adminClient.CreateTopicsAsync([topicSpecification]);

                    _logger.LogInformation("Default topic initialized successfully.");
                }
            });
    }
}