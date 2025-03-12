using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(d => d.DesignIdea).WithMany(p => p.ProductDetails).HasForeignKey(d => d.DesignIdeaId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(d => d.Product).WithMany(p => p.ProductDetails).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
