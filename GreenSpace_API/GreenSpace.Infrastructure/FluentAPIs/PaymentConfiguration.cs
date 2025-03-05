using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(e => e.PaymentId);
        builder.Property(e => e.PaymentId).ValueGeneratedNever();
        builder.Property(e => e.PaymentMethod).HasMaxLength(100);
    }
}
