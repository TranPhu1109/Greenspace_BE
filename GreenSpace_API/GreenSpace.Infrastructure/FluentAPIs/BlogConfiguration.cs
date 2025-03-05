using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.HasKey(e => e.BlogId).HasName("PK__Blog__54379E300A31CB42");
            builder.ToTable("Blog");

            builder.Property(e => e.BlogId).ValueGeneratedNever();
            builder.Property(e => e.Author).HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(250);
            builder.Property(e => e.Title).HasMaxLength(200);

            builder.HasOne(d => d.Image).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("FK__Blog__ImageId__6383C8BA");
        }
    }
}
