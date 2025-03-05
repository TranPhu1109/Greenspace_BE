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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.OrderId);
            builder.Property(e => e.OrderId).ValueGeneratedNever();
            builder.Property(e => e.OrderDate);
            builder.Property(e => e.Phone).HasMaxLength(15);
            builder.HasOne(d => d.Payment).WithMany(p => p.Orders).HasForeignKey(d => d.PaymentId);
            builder.HasOne(d => d.User).WithMany(p => p.Orders).HasForeignKey(d => d.UserId);
        }
    }
}
