using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(e => e.BillId);
        builder.HasOne(d => d.Order).WithMany(p => p.Bills).HasForeignKey(d => d.OrderId);
        builder.HasOne(d => d.Payment).WithMany(p => p.Bills).HasForeignKey(d => d.PaymentId);
        builder.HasOne(d => d.ServiceOrder).WithMany(p => p.Bills).HasForeignKey(d => d.ServiceOrderId);
    }
}
