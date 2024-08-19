namespace TechDemo.Infrastructure.EntityFramework;

public class TechDemoOptions
{
    public const string Key = nameof(TechDemoOptions);
    public string ConnectionString { get; set; } = string.Empty;
}