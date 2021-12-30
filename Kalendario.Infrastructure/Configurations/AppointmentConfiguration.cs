﻿using Kalendario.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Persistence.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId);
            
            builder.HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId);
                        
            builder.HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId);
        }
    }
}