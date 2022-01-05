using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class ScheduleFrameConfiguration : IEntityTypeConfiguration<ScheduleFrame>
{
    public void Configure(EntityTypeBuilder<ScheduleFrame> builder)
    {
        builder.Property(s => s.Order)
            .IsRequired();

        builder.Property(s => s.Start)
            .IsRequired();

        builder.Property(s => s.End)
            .IsRequired();

        builder.HasIndex(s => new {s.ScheduleId, s.Offset, s.Order})
            .IsUnique();
    }
}