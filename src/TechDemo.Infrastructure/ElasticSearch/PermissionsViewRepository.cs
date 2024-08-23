using Microsoft.Extensions.Options;
using Nest;
using TechDemo.Domain.Permissions.ViewModels;
using TechDemo.Domain.Shared.Results;
using Result = TechDemo.Domain.Shared.Results.Result;

namespace TechDemo.Infrastructure.ElasticSearch;

internal class PermissionsViewRepository : IPermissionsViewRepository
{
    private readonly IElasticClient _elasticClient;
    private readonly ElasticSearchOptions _options;

    public PermissionsViewRepository(
        IElasticClient elasticClient, IOptions<ElasticSearchOptions> options)
    {
        _elasticClient = elasticClient ??
            throw new ArgumentNullException(nameof(elasticClient));
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<Result<IEnumerable<PermissionViewModel>>> GetAsync(
        string term, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _elasticClient.SearchAsync<PermissionViewModel>(
            search => search.Query(query => query.QueryString(
                queryString => queryString.Query($"*{term}*")))
                .Size(_options.DefaultResultNumber)
        );

        return result.IsValid
            ? Result<IEnumerable<PermissionViewModel>>.Success(result.Documents)
            : Result<IEnumerable<PermissionViewModel>>.Failure(ElasticSearchErrors.QueryError);
    }

    public async Task<Result<None>> AddAsync(
        PermissionViewModel permission, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var response = await _elasticClient.IndexDocumentAsync(permission);

        return response.IsValid
            ? Result.Success()
            : Result.Failure(ElasticSearchErrors.AddViewError);
    }

    public async Task<Result<None>> UpdateAsync(
        PermissionViewModel permission, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var response = await _elasticClient.UpdateAsync<PermissionViewModel>(
            permission.Id, descriptor => descriptor.Doc(permission));

        return response.IsValid
            ? Result.Success()
            : Result.Failure(ElasticSearchErrors.UpdateViewError);
    }
}