using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(e => new { e.OrderId, e.ProductId });
        builder.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasForeignKey(d => d.OrderId);
        builder.HasOne(d => d.Product).WithMany(p => p.OrderDetails).HasForeignKey(d => d.ProductId);
    }
}
