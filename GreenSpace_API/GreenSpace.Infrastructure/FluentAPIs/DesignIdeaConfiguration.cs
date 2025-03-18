using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class DesignIdeaConfiguration : IEntityTypeConfiguration<DesignIdea>
{
    public void Configure(EntityTypeBuilder<DesignIdea> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(d => d.DesignIdeasCategory).WithMany(p => p.DesignIdeas).HasForeignKey(d => d.DesignIdeasCategoryId);
        builder.HasOne(d => d.Image).WithMany(p => p.DesignIdeas).HasForeignKey(d => d.ImageId);
    }
}
