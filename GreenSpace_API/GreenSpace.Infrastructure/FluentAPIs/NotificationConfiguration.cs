using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(e => e.NotificationId);
            builder.Property(e => e.NotificationId).ValueGeneratedNever();
            builder.Property(e => e.Message).HasMaxLength(200);
            builder.Property(e => e.Title).HasMaxLength(100);
            builder.HasOne(d => d.User).WithMany(p => p.Notifications).HasForeignKey(d => d.UserId);
        }
    }
}
