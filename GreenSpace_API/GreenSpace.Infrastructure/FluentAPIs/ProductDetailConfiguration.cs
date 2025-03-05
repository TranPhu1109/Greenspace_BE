using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.HasKey(e => new { e.ProductId, e.DesignIdeaId });
            builder.HasOne(d => d.DesignIdea).WithMany(p => p.ProductDetails).HasForeignKey(d => d.DesignIdeaId);
            builder.HasOne(d => d.Product).WithMany(p => p.ProductDetails).HasForeignKey(d => d.ProductId);
        }
    }
}
