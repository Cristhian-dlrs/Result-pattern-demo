namespace TechDemo.Infrastructure.EntityFramework.Outbox;

internal sealed record OutboxMessage(
    Guid Id,
    string Operation,
    string Payload,
    DateTime OcurredOn,
    DateTime? ProcessedOn
)
{
    public DateTime? ProcessedOn { get; set; } = ProcessedOn;
}