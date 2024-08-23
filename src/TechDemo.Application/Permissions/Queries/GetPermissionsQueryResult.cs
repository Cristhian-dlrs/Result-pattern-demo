using TechDemo.Domain.Permissions.ViewModels;

namespace TechDemo.Application.Permissions.Queries;

public record GetPermissionsQueryResult(
    IEnumerable<PermissionViewModel> Permissions
);