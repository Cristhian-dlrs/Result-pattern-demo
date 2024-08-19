using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechDemo.Domain.Permissions.Models;
using TechDemo.Infrastructure.EntityFramework;
using TechDemo.Infrastructure.Repositories;

namespace TechDemo.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("PermissionsDb")));

        services.AddSingleton<IPermissionsRepository, PermissionsRepository>();
        return services;
    }
}