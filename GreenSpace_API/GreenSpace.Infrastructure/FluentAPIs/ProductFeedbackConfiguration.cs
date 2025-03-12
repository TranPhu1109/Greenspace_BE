using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class ProductFeedbackConfiguration : IEntityTypeConfiguration<ProductFeedback>
{
    public void Configure(EntityTypeBuilder<ProductFeedback> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(d => d.Product).WithMany(p => p.MaterialFeedbacks).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.User).WithMany(p => p.MaterialFeedbacks).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
