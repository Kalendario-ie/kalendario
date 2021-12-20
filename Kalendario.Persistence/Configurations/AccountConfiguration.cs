using Kalendario.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasMany(a => a.Employees)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);

            builder.HasMany(a => a.Services)
                .WithOne(e => e.Account)
                .HasForeignKey(e => e.AccountId);
        }
    }
}