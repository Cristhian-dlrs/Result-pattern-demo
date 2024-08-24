using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechDemo.Domain.Permissions.Models;

namespace TechDemo.Infrastructure.EntityFramework.Configurations;

internal class PermissionTypeConfiguration : IEntityTypeConfiguration<PermissionType>
{
    public void Configure(EntityTypeBuilder<PermissionType> builder)
    {
        builder.ToTable("PermissionTypes");
        builder.HasKey(permissionType => permissionType.Id);
        builder.Property(permissionType => permissionType.Id).ValueGeneratedOnAdd();
        builder.Property(permissionType => permissionType.Description).HasMaxLength(20);
        builder.HasData([.. PermissionType.PermissionTypes.ToArray()]);
    }
}