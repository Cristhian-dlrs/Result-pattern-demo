using Nest;
using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Results;
using Result = TechDemo.Domain.Shared.Results.Result;

namespace TechDemo.Infrastructure.ElasticSearch;

internal class PermissionsViewRepository : IPermissionsViewRepository
{
    private readonly IElasticClient _elasticClient;

    public PermissionsViewRepository(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient ??
            throw new ArgumentNullException(nameof(elasticClient));
    }

    public async Task<Result<IEnumerable<PermissionViewModel>>> GetAsync(
        string term, int resultNumber, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _elasticClient.SearchAsync<PermissionViewModel>(
            search => search.Query(query => query.QueryString(
                queryString => queryString.Query($"*{term}*")))
                .Size(resultNumber)
        );

        return result.IsValid
            ? Result<IEnumerable<PermissionViewModel>>.Success(result.Documents)
            : Result<IEnumerable<PermissionViewModel>>.Failure(Error.QueryError);
    }

    public async Task<Result<Empty>> AddAsync(
        PermissionViewModel permission, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var response = await _elasticClient.IndexDocumentAsync(permission);

        return response.IsValid
            ? Result.Success()
            : Result.Failure(Error.AddViewError);
    }

    public async Task<Result<Empty>> UpdateAsync(
        PermissionViewModel permission, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var response = await _elasticClient.UpdateAsync<PermissionViewModel>(
            permission.Id, descriptor => descriptor.Doc(permission));

        return response.IsValid
            ? Result.Success()
            : Result.Failure(Error.UpdateViewError);
    }
}