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
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasKey(e => e.BillId).HasName("PK__Bill__11F2FC6AECC46357");

            builder.ToTable("Bill");

            builder.Property(e => e.BillId).ValueGeneratedNever();
            builder.Property(e => e.Description).HasColumnType("text");

            builder.HasOne(d => d.Order).WithMany(p => p.Bills)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Bill__OrderId__74AE54BC");

            builder.HasOne(d => d.Payment).WithMany(p => p.Bills)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__Bill__PaymentId__72C60C4A");

            builder.HasOne(d => d.ServiceOrder).WithMany(p => p.Bills)
                .HasForeignKey(d => d.ServiceOrderId)
                .HasConstraintName("FK__Bill__ServiceOrd__73BA3083");
        }
    }
}
