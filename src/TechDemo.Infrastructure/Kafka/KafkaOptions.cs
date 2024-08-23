using System.ComponentModel.DataAnnotations;

namespace TechDemo.Infrastructure.Kafka;

public class KafkaOptions
{
    [Required]
    public string BootstrapServers { get; set; } = string.Empty;
    [Required]
    public string DefaultTopic { get; set; } = string.Empty;
    [Required]
    public int BatchSize { get; set; }
}