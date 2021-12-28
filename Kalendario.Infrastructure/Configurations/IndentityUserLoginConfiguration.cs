using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations;

public class IndentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
    {
        builder.Property<string>(i => i.LoginProvider)
            .HasMaxLength(128)
            .HasColumnType("character varying(128)");

        builder.Property<string>(i => i.ProviderKey)
            .HasMaxLength(128)
            .HasColumnType("character varying(128)");
    }
}