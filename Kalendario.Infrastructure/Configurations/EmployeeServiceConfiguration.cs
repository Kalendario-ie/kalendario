using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class EmployeeServiceConfiguration : IEntityTypeConfiguration<EmployeeService>
{
    public void Configure(EntityTypeBuilder<EmployeeService> builder)
    {
        builder.ToTable("EmployeeService");

        builder.HasOne(es => es.Employee)
            .WithMany(e => e.EmployeeServices)
            .HasForeignKey(es => es.EmployeeId);
        
        builder.HasOne(es => es.Service)
            .WithMany(s => s.EmployeeServices)
            .HasForeignKey(es => es.ServiceId);
    }
}