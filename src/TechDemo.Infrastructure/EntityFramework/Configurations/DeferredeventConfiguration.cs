
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechDemo.Infrastructure.EntityFramework.Configurations;

internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<DeferredEvent>
{
    public void Configure(EntityTypeBuilder<DeferredEvent> builder)
    {
        builder.ToTable("DeferredEvents");
    }
}
