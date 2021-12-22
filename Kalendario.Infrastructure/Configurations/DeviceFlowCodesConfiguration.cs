﻿using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kalendario.Infrastructure.Configurations
{
    public class DeviceFlowCodesConfiguration : IEntityTypeConfiguration<DeviceFlowCodes>
    {
        public void Configure(EntityTypeBuilder<DeviceFlowCodes> builder)
        {
            builder.ToTable("DeviceCodes");

            builder.Property(x => x.DeviceCode).HasMaxLength(200).IsRequired();
            builder.Property(x => x.UserCode).HasMaxLength(200).IsRequired();
            builder.Property(x => x.SubjectId).HasMaxLength(200);
            builder.Property(x => x.SessionId).HasMaxLength(100);
            builder.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.CreationTime).IsRequired();
            builder.Property(x => x.Expiration).IsRequired();
            // 50000 chosen to be explicit to allow enough size to avoid truncation, yet stay beneath the MySql row size limit of ~65K
            // apparently anything over 4K converts to nvarchar(max) on SqlServer
            builder.Property(x => x.Data).HasMaxLength(50000).IsRequired();

            builder.HasKey(x => new {x.UserCode});

            builder.HasIndex(x => x.DeviceCode).IsUnique();
            builder.HasIndex(x => x.Expiration);
        }
    }
}