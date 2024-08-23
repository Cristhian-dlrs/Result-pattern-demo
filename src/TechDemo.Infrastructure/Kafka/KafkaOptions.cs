using System.ComponentModel.DataAnnotations;

namespace TechDemo.Infrastructure.Kafka;

public class KafkaOptions
{
    [Required]
    public string BootstrapServers { get; set; } = string.Empty;

    [Required]
    public string DefaultGroupId { get; set; }

    [Required]
    public string DefaultTopic { get; set; } = string.Empty;

    [Required]
    public bool AllowAutoCreateTopics { get; set; }

    [Required]
    public int PartitionsNumber { get; set; }

    [Required]
    public short ReplicationFactor { get; set; }

    [Required]
    public int BatchSize { get; set; }
}