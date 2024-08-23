using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechDemo.Domain.Permissions.Models;

namespace TechDemo.Infrastructure.EntityFramework.Configurations;

internal class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(permission => permission.Id);
        builder.Property(permission => permission.Id)
        .ValueGeneratedOnAdd()
        .UseIdentityColumn();

        builder.Property(permission => permission.PermissionDate).IsRequired();

        builder.HasOne<PermissionType>()
            .WithMany()
            .HasForeignKey(permissionType => permissionType.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .Property(permission => permission.PermissionType)
            .HasConversion(
                permissionType => permissionType.Id,
                value => PermissionType.FromId(value).Value);

        builder.Property(permission => permission.EmployeeForename)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(permission => permission.EmployeeSurname)
            .HasMaxLength(100)
            .IsRequired();
    }
}
