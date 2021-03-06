using Kalendario.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(i => i.Name)
                .IsUnique();

            builder.HasMany(a => a.EmployeeServices)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);

            builder.HasMany(a => a.Employees)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);

            builder.HasMany(a => a.Services)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);
            
            builder.HasMany(a => a.ServiceCategories)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);
            
            builder.HasMany(a => a.Customers)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);

            builder.HasMany(a => a.Appointments)
                .WithOne(a => a.Account)
                .HasForeignKey(a => a.AccountId);
        }
    }
}