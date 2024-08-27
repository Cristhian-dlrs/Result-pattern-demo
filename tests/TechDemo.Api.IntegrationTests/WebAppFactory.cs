using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Elasticsearch;
using Testcontainers.Kafka;
using Testcontainers.SqlEdge;

namespace TechDemo.Api.IntegrationTests;

public class WebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly SqlEdgeContainer _sqlServerContainer;
    private readonly IContainer _zookeeperContainer;
    private readonly KafkaContainer _kafkaContainer;
    private readonly ElasticsearchContainer _elasticsearchContainer;
    private readonly INetwork _appNetwork;

    public WebAppFactory()
    {
        _appNetwork = new NetworkBuilder()
            .WithName("app-network")
            .Build();

        _sqlServerContainer = new SqlEdgeBuilder()
            .WithName("sqlserver")
            .WithImage("mcr.microsoft.com/azure-sql-edge")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithPortBinding(1433, 1433)
            .WithNetwork(_appNetwork)
            .WithWaitStrategy(Wait.ForUnixContainer())
            .Build();

        _zookeeperContainer = new ContainerBuilder()
            .WithImage("confluentinc/cp-zookeeper")
            .WithName("zookeeper")
            .WithPortBinding(2181, 2181)
            .WithEnvironment("ZOOKEEPER_CLIENT_PORT", "2181")
            .WithEnvironment("ZOOKEEPER_TICK_TIME", "2000")
            .WithNetwork(_appNetwork)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(2181))
            .Build();

        _kafkaContainer = new KafkaBuilder()
            .WithName("kafka")
            .WithImage("confluentinc/cp-kafka")
            .WithEnvironment("KAFKA_INTER_BROKER_LISTENER_NAME", "INSIDE")
            .WithEnvironment("KAFKA_ADVERTISED_LISTENERS", "INSIDE://kafka:29092,OUTSIDE://localhost:9092")
            .WithEnvironment("KAFKA_LISTENERS", "INSIDE://0.0.0.0:29092,OUTSIDE://0.0.0.0:9092")
            .WithEnvironment("KAFKA_LISTENER_SECURITY_PROTOCOL_MAP", "INSIDE:PLAINTEXT,OUTSIDE:PLAINTEXT")
            .WithEnvironment("KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1")
            .WithEnvironment("KAFKA_ZOOKEEPER_CONNECT", "zookeeper:2181")
            .DependsOn(_zookeeperContainer)
            .WithPortBinding(9092, 9092)
            .WithNetwork(_appNetwork)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9092))
            .Build();

        _elasticsearchContainer = new ElasticsearchBuilder()
            .WithName("elasticsearch")
            .WithImage("docker.elastic.co/elasticsearch/elasticsearch:8.7.1")
            .WithEnvironment("discovery.type", "single-node")
            .WithEnvironment("xpack.security.enabled", "false")
            .WithPortBinding(9200, 9200)
            .WithNetwork(_appNetwork)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9200))
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _zookeeperContainer.StartAsync();
        await _kafkaContainer.StartAsync();
        await _elasticsearchContainer.StartAsync();
        await _sqlServerContainer.StartAsync();
    }

    public async new Task DisposeAsync()
    {
        await _elasticsearchContainer.StopAsync();
        await _zookeeperContainer.StopAsync();
        await _kafkaContainer.StopAsync();
        await _sqlServerContainer.StopAsync();
    }
}