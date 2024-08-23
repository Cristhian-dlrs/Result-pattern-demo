using MediatR;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Application.Permissions.Queries;

public record GetPermissionsQuery(
    string? SearchTerm) : IRequest<Result<GetPermissionsQueryResult>>;
