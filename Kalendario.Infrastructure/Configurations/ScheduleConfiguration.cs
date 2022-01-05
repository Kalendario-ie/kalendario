using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.Ignore(s => s.Sunday);
        builder.Ignore(s => s.Monday);
        builder.Ignore(s => s.Tuesday);
        builder.Ignore(s => s.Wednesday);
        builder.Ignore(s => s.Thursday);
        builder.Ignore(s => s.Friday);
        builder.Ignore(s => s.Saturday);

        builder.HasMany(s => s.Frames)
            .WithOne(sc => sc.Schedule)
            .HasForeignKey(s => s.ScheduleId)
            .IsRequired();
    }
}