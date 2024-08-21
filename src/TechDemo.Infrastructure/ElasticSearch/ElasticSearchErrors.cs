using TechDemo.Domain.Shared.Results;

namespace TechDemo.Infrastructure.ElasticSearch;

public sealed record ElasticSearchErrors(string Code, string Description)
{
    public static readonly Error AddViewError = new(
        "ElasticSearchErrors.AddViewError",
        "Unable to add the view model.");

    public static readonly Error UpdateViewError = new(
        "ElasticSearchErrors.UpdateViewError",
        "Unable to update vew model.");

    public static readonly Error QueryError = new(
        "ElasticSearchErrors.QueryError",
        "Unable to perform the query.");
}