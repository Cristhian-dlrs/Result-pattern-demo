namespace TechDemo.Infrastructure.Kafka;

internal class KafkaOptions
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string DefaultTopic { get; set; } = string.Empty;
    public int BatchSize { get; set; }
}