using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.HasKey(e => e.ProductId);
        builder.Property(e => e.ProductId).ValueGeneratedNever();
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.HasOne(d => d.Category).WithMany(p => p.Materials).HasForeignKey(d => d.CategoryId);
        builder.HasOne(d => d.Image).WithMany(p => p.Materials).HasForeignKey(d => d.ImageId);
    }
}
