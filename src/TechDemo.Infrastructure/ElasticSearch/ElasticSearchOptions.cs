using System.ComponentModel.DataAnnotations;

namespace TechDemo.Infrastructure.ElasticSearch;

class ElasticSearchOptions
{
    [Required]
    public string Url { get; set; } = string.Empty;
    [Required]
    public string DefaultIndex { get; set; } = string.Empty;
    [Required]
    public int DefaultResultNumber { get; set; }
}