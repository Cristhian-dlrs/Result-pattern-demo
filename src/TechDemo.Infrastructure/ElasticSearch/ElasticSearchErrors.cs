using TechDemo.Domain.Shared.Results;

namespace TechDemo.Infrastructure.ElasticSearch;

public static class ElasticSearchErrors
{
    public static readonly Error AddViewError = new(
        "ElasticSearchErrors.AddViewError",
        "Unable to add the view model.");

    public static readonly Error IndexAlreadyCreated = new(
        "resource_already_exists_exception",
        "The index already exists and does not needs to be created.");

    public static readonly Error UpdateViewError = new(
        "ElasticSearchErrors.UpdateViewError",
        "Unable to update vew model.");

    public static readonly Error QueryError = new(
        "ElasticSearchErrors.QueryError",
        "Unable to perform the query.");
}