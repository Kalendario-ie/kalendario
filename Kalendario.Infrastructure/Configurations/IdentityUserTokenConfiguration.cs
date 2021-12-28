using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
    {
        builder.Property<string>(i => i.LoginProvider)
            .HasMaxLength(128)
            .HasColumnType("character varying(128)");

        builder.Property<string>(i => i.Name)
            .HasMaxLength(128)
            .HasColumnType("character varying(128)");
    }
}