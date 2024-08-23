using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechDemo.Api.Permissions.Requests;
using TechDemo.Application.Permissions.Commands;
using TechDemo.Application.Permissions.Queries;
using TechDemo.Domain.Shared.Results;

namespace TechDemo.Api.Permissions.Endpoints;

public static class PermissionsEndpoints
{
    public static void AddPermissionsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/permissions",
            async (
                CancellationToken cancellationToken,
                ISender _sender,
                [FromQuery] string? searchTerm) =>
        {
            return await _sender.Send(
                    new GetPermissionsQuery(searchTerm),
                    cancellationToken)
                .Unwrap()
                .MatchAsync(
                    onSuccess: result => Task.FromResult(Results.Created()),
                    onFailure: error => Task.FromResult(Results.Problem(detail: error.Description)));
        })
        .WithName("GetPermissions");

        app.MapPost(
            "/permissions",
            async (
                CancellationToken cancellationToken,
                ISender _sender,
                [FromBody] CreatePermissionRequest request) =>
        {
            return await _sender.Send(new RequestPermissionsCommand(
                    request.EmployeeForename,
                    request.EmployeeSurname,
                    request.PermissionType), cancellationToken)
                .Unwrap()
                .MatchAsync(
                    onSuccess: result => Task.FromResult(Results.NoContent()),
                    onFailure: error => Task.FromResult(Results.Problem(detail: error.Description)));
        })
        .WithName("CreatePermissions");

        app.MapPatch(
            "/permissions/{permissionId:int}",
            async (
                CancellationToken cancellationToken,
                ISender _sender, int permissionId,
                [FromBody] ModifyPermissionRequest request) =>
        {
            return await _sender.Send(new ModifyPermissionsCommand(
                    permissionId,
                    request.EmployeeForename,
                    request.EmployeeSurname,
                    request.PermissionType), cancellationToken)
                .Unwrap()
                .Match(
                    onSuccess: result => Task.FromResult(Results.NoContent()),
                    onFailure: error => Task.FromResult(Results.Problem(detail: error.Description)));
        }).
        WithName("ModifyPermissions");
    }
}