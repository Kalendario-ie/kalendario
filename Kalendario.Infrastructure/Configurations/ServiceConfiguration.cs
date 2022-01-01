using Kalendario.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(s => s.Description)
            .HasMaxLength(255);

        builder.Property(s => s.Price)
            .HasPrecision(2);

        builder.HasOne(s => s.ServiceCategory)
            .WithMany(sc => sc.Services)
            .HasForeignKey(s => s.ServiceCategoryId)
            .IsRequired(false);
    }
}