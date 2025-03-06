using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(d => d.Category).WithMany(p => p.Materials).HasForeignKey(d => d.CategoryId);
        builder.HasOne(d => d.Image).WithMany(p => p.Materials).HasForeignKey(d => d.ImageId);
    }
}
