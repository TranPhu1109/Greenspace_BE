using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class ServiceOrderDetailConfiguration : IEntityTypeConfiguration<ServiceOrderDetail>
{
    public void Configure(EntityTypeBuilder<ServiceOrderDetail> builder)
    {
        builder.HasKey(e => new { e.ServiceOrderId, e.ProductId });
        builder.HasOne(d => d.Product).WithMany(p => p.ServiceOrderDetails).HasForeignKey(d => d.ProductId);
        builder.HasOne(d => d.ServiceOrder).WithMany(p => p.ServiceOrderDetails).HasForeignKey(d => d.ServiceOrderId);
    }
}
