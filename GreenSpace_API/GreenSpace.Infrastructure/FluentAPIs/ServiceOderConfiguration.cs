using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class ServiceOderConfiguration : IEntityTypeConfiguration<ServiceOrder>
{
    public void Configure(EntityTypeBuilder<ServiceOrder> builder)
    {
        builder.HasKey(e => e.ServiceOrderId);
        builder.Property(e => e.ServiceOrderId).ValueGeneratedNever();
        builder.Property(e => e.Address).HasMaxLength(200);
        builder.Property(e => e.CusPhone).HasMaxLength(13);
        builder.Property(e => e.Description);
        builder.HasOne(d => d.ServiceType).WithMany(p => p.ServiceOrders).HasForeignKey(d => d.ServiceTypeId);
    }
}
