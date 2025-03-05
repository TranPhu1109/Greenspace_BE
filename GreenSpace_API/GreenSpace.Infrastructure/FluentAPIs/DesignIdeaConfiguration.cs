using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class DesignIdeaConfiguration : IEntityTypeConfiguration<DesignIdea>
{
    public void Configure(EntityTypeBuilder<DesignIdea> builder)
    {
        builder.HasKey(e => e.DesignIdeaId);
        builder.Property(e => e.DesignIdeaId).ValueGeneratedNever();
        builder.Property(e => e.Name).HasMaxLength(100);
        builder.HasOne(d => d.Category).WithMany(p => p.DesignIdeas)
                .HasForeignKey(d => d.CategoryId);
        builder.HasOne(d => d.Image).WithMany(p => p.DesignIdeas)
                .HasForeignKey(d => d.ImageId);
    }
}
