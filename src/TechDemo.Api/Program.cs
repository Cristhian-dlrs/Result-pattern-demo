using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using TechDemo.Api.Middleware;
using TechDemo.Api.Permissions.Endpoints;
using TechDemo.Application;
using TechDemo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddEndpointsApiExplorer()
        .AddInfrastructureServices()
        .AddApplicationServices()
        .Configure<JsonOptions>(options =>
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
}

var app = builder.Build();
{
    app.UseGlobalExceptionHandler();
}

var permissionGroup = app.MapGroup("/api/v1");
permissionGroup.AddPermissionsEndpoints();
app.Run("http://*:5001");