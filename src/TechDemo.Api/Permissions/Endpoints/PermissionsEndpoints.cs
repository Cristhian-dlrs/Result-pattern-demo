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
            async (ISender _sender, [FromQuery] string searchTerm) =>
        {
            return await _sender.Send(new GetPermissionsQuery(searchTerm))
                .Unwrap()
                .MatchAsync(
                    onSuccess: result => Task.FromResult(Results.Ok(result)),
                    onFailure: error => Task.FromResult(Results.Problem(detail: error.Description)));
        });

        app.MapPost(
            "/permissions",
            async (ISender _sender, [FromBody] CreatePermissionRequest request) =>
        {
            return await _sender.Send(new RequestPermissionsCommand(
                    request.EmployeeForename,
                    request.EmployeeSurname,
                    request.PermissionType))
                .Unwrap()
                .MatchAsync(
                    onSuccess: result => Task.FromResult(Results.NoContent()),
                    onFailure: error => Task.FromResult(Results.Problem(detail: error.Description)));
        });

        app.MapPatch(
            "/permissions/{permissionId:int}",
            async (ISender _sender, int permissionId, [FromBody] ModifyPermissionRequest request) =>
        {
            return await _sender.Send(new ModifyPermissionsCommand(
                    permissionId,
                    request.EmployeeForename,
                    request.EmployeeSurname,
                    request.PermissionType))
                .Unwrap()
                .MatchAsync(
                    onSuccess: result => Task.FromResult(Results.NoContent()),
                    onFailure: error => Task.FromResult(Results.Problem(detail: error.Description)));
        });
    }
}