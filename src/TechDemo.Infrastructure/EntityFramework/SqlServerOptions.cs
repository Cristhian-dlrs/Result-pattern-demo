using System.ComponentModel.DataAnnotations;

namespace TechDemo.Infrastructure.EntityFramework;

internal class SqlServerOptions
{
    [Required]
    public string ConnectionString { get; set; } = string.Empty;
}