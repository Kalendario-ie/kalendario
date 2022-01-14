using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class SchedulingPanelConfiguration : IEntityTypeConfiguration<SchedulingPanel>
{
    public void Configure(EntityTypeBuilder<SchedulingPanel> builder)
    {
        builder.HasMany(sp => sp.Employees)
            .WithMany(e => e.SchedulingPanels);
    }
}