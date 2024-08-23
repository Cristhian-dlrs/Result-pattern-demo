using TechDemo.Api.Middleware;
using TechDemo.Application;
using TechDemo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services
        .AddInfrastructureServices(builder.Configuration)
        .AddApplicationServices();
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseGlobalExceptionHandler();
}

app.Run();