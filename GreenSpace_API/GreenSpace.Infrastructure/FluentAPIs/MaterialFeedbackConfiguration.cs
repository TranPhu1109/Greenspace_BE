using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class MaterialFeedbackConfiguration : IEntityTypeConfiguration<MaterialFeedback>
{
    public void Configure(EntityTypeBuilder<MaterialFeedback> builder)
    {
        builder.HasKey(e => new { e.UserId, e.ProductId });
        builder.HasOne(d => d.Product).WithMany(p => p.MaterialFeedbacks).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.User).WithMany(p => p.MaterialFeedbacks).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
