using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechDemo.Infrastructure.EntityFramework;

namespace TechDemo.Infrastructure.Kafka;

public class InitializationService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    ILogger<InitializationService> _logger;

    public InitializationService(
        IServiceScopeFactory serviceScopeFactory, ILogger<InitializationService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Initializing Services...");
        await InitializeSqlDbAsync();
        await InitializeKafkaAsync();
    }

    private async Task InitializeSqlDbAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<AppDbContext>();

            try
            {
                _logger.LogInformation("Starting to apply database migrations...");

                await dbContext.Database.MigrateAsync();

                _logger.LogInformation("Database migrations applied successfully.");
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Database initialization was cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying database migrations.");
                throw;
            }
        }
    }

    private async Task InitializeKafkaAsync()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var services = scope.ServiceProvider;
            var adminClient = services.GetRequiredService<IAdminClient>();
            var options = services.GetRequiredService<IOptions<KafkaOptions>>().Value;

            try
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
            }
            catch (CreateTopicsException ex) when (ex.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
            {
                _logger.LogInformation(ex, $"Topic {options.DefaultTopic} already exists.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating Kafka topic: {options.DefaultTopic}");
            }
        }
    }
}