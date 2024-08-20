namespace TechDemo.Infrastructure.ElasticSearch;

class ElasticSearchOptions
{
    public string Url { get; set; } = string.Empty;
    public string DefaultIndex { get; set; } = string.Empty;
}