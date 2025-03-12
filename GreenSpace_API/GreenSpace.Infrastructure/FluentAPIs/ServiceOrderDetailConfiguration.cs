using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class ServiceOrderDetailConfiguration : IEntityTypeConfiguration<ServiceOrderDetail>
{
    public void Configure(EntityTypeBuilder<ServiceOrderDetail> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(d => d.Product).WithMany(p => p.ServiceOrderDetails).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.ServiceOrder).WithMany(p => p.ServiceOrderDetails).HasForeignKey(d => d.ServiceOrderId).OnDelete(DeleteBehavior.Restrict);
    }
}
