using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Serilog;
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

    builder.Host.UseSerilog((context, config) =>
        config.ReadFrom.Configuration(context.Configuration));
}

var app = builder.Build();
{
    app.UseSerilogRequestLogging();
    app.UseGlobalExceptionHandler();
}

var permissionGroup = app.MapGroup("/api/v1");
permissionGroup.AddPermissionsEndpoints();
app.Run("http://*:5001");

public partial class Program;