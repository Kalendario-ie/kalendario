using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations
{
    public class KeyConfiguration : IEntityTypeConfiguration<Key>
    {
        public void Configure(EntityTypeBuilder<Key> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Use);
            builder.Property(x => x.Algorithm).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Data).IsRequired();
        }
    }
}