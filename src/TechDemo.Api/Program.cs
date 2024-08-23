using TechDemo.Api.Middleware;
using TechDemo.Application;
using TechDemo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services
        .AddInfrastructureServices()
        .AddApplicationServices();
}

var app = builder.Build();
{
    await app.Services.InitializeSqlDbAsync<Program>();
    app.UseHttpsRedirection();
    app.UseGlobalExceptionHandler();
}

await app.RunAsync();


// dotnet ef migrations add AddDeferredMessages --project ./src/TechDemo.infrastructure/ --startup-project ./src/TechDemo.api/ --output-dir EntityFramework/migrations