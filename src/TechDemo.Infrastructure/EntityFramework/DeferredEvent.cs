namespace TechDemo.Infrastructure.EntityFramework;

internal sealed record DeferredEvent(
    Guid Id,
    string Payload,
    DateTime RegisteredOn,
    DateTime? ProcessedOn
)
{
    public DateTime? ProcessedOn { get; set; } = ProcessedOn;
}