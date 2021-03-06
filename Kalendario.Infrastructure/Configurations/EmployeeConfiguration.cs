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
                            .WithMany(service => service.EmployeeServices)
                            .HasForeignKey(es => es.ServiceId),
                    eb => eb.HasOne(es => es.Employee)
                        .WithMany(employee => employee.EmployeeServices)
                        .HasForeignKey(es => es.EmployeeId)
                ).HasKey(s => s.Id);

            builder.HasOne(e => e.Schedule)
                .WithMany(s => s.Employees)
                .HasForeignKey(employee => employee.ScheduleId)
                .IsRequired(false);
        }
    }
}