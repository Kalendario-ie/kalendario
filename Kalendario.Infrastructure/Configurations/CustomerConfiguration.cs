using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(s => s.Warning)
            .HasMaxLength(255);

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(255);
        
        builder.Property(s => s.Email)
            .HasMaxLength(255);
    }
}