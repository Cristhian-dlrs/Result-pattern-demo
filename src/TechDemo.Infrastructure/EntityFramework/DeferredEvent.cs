namespace TechDemo.Infrastructure.EntityFramework;

public class DeferredEvent
{
    public Guid Id { get; set; }
    public string Payload { get; set; }
    public DateTime RegisteredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
}