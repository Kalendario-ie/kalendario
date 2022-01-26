using Kalendario.Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class ApplicationRoleGroupConfiguration : IEntityTypeConfiguration<ApplicationRoleGroup>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleGroup> builder)
    {
        builder.HasOne(s => s.Account)
            .WithMany()
            .HasForeignKey(s => s.AccountId);

        builder.HasMany(s => s.Roles)
            .WithMany(r => r.RoleGroups)
            .UsingEntity<RoleGroupRole>(
                eb => eb.HasOne(rgr => rgr.Role)
                    .WithMany(role => role.RoleGroupRoles)
                    .HasForeignKey(es => es.RoleId),
                eb => eb.HasOne(es => es.RoleGroup)
                    .WithMany(rg => rg.RoleGroupRoles)
                    .HasForeignKey(es => es.RoleGroupId)
            ).HasKey(s => s.Id);


        builder.Property(r => r.Name)
            .HasMaxLength(120);
        
        builder.Ignore(e => e.DomainEvents);
    }
}