namespace TechDemo.Domain.Shared.Models;

public interface IDomainEvent
{
    public Guid Id { get; }
    public string Operation { get; }
}