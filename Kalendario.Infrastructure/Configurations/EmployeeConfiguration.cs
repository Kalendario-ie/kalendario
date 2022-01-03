using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasMany<Service>(e => e.Services)
                .WithMany(s => s.Employees)
                .UsingEntity<EmployeeService>(
                    eb => eb.HasOne(es => es.Service)
                            .WithMany()
                            .HasForeignKey(es => es.ServiceId),
                    eb => eb.HasOne(es => es.Employee)
                        .WithMany()
                        .HasForeignKey(es => es.EmployeeId)
                ).HasKey(s => s.Id);
        }
    }
}