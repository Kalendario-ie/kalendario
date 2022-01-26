using Kalendario.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(u => u.Account)
            .WithMany(a => a.Users)
            .HasForeignKey(u => u.AccountId)
            .IsRequired(false);

        builder.HasOne(a => a.RoleGroup)
            .WithMany()
            .HasForeignKey(a => a.RoleGroupId)
            .IsRequired(false);
        
        builder.HasOne(a => a.Employee)
            .WithMany()
            .HasForeignKey(a => a.EmployeeId)
            .IsRequired(false);
    }
}