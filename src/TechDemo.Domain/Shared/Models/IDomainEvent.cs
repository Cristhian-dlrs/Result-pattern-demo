namespace TechDemo.Domain.Shared.Models;

public interface IDomainEvent
{
    public Guid CorrelationId { get; }
    public string Operation { get; }
    // public DateTime OccurredOn { get; }
    // public bool Processed { get; set; }
}