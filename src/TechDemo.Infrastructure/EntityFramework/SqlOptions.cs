using System.ComponentModel.DataAnnotations;

namespace TechDemo.Infrastructure.EntityFramework;

internal class SqlOptions
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
}