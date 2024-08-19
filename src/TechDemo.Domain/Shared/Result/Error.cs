namespace TechDemo.Domain.Shared.Result;

public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Null Value", "The value cannot be null.");
}