using TechDemo.Api.Middleware;
using TechDemo.Api.Permissions.Endpoints;
using TechDemo.Application;
using TechDemo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddEndpointsApiExplorer()
        .AddInfrastructureServices()
        .AddApplicationServices();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseGlobalExceptionHandler();
}

var permissionGroup = app.MapGroup("/api/v1");
permissionGroup.AddPermissionsEndpoints();

app.Run();

// dotnet ef migrations add AddDeferredMessages --project ./src/TechDemo.infrastructure/ --startup-project ./src/TechDemo.api/ --output-dir EntityFramework/migrations