using Microsoft.EntityFrameworkCore;
using TechDemo.Domain.Permissions.Models;

namespace TechDemo.Infrastructure.EntityFramework;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<PermissionType> PermissionTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
