using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.HasKey(e => e.BlogId);
        builder.HasOne(d => d.Image).WithMany(p => p.Blogs).HasForeignKey(d => d.ImageId);
    }
}
