using Nest;
using TechDemo.Domain.Permissions.ViewModels;

namespace TechDemo.Infrastructure.ElasticSearch;

internal class PermissionsViewRepository : IPermissionsViewRepository
{
    private readonly IElasticClient _elasticClient;

    public PermissionsViewRepository(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient ??
            throw new ArgumentNullException(nameof(elasticClient));
    }

    public async Task<IEnumerable<PermissionViewModel>?> GetAsync(
        string term, int resultNumber, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _elasticClient.SearchAsync<PermissionViewModel>(
            search => search.Query(query => query.QueryString(
                queryString => queryString.Query($"*{term}*")))
                .Size(resultNumber)
        );

        return result.IsValid
            ? [.. result.Documents]
            : default;
    }

    public async Task<bool> InsertAsync(
        PermissionViewModel permission, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var response = await _elasticClient.IndexDocumentAsync(permission);
        return response.IsValid;
    }

    public async Task<bool> UpdateAsync(
        PermissionViewModel permission, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var response = await _elasticClient.UpdateAsync<PermissionViewModel>(
            permission.Id, descriptor => descriptor.Doc(permission));
        return response.IsValid;
    }
}