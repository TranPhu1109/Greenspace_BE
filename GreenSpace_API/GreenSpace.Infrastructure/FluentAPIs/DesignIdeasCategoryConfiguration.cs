using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class DesignIdeasCategoryConfiguration : IEntityTypeConfiguration<DesignIdeasCategory>
{
    public void Configure(EntityTypeBuilder<DesignIdeasCategory> builder)
    {
        builder.HasKey(e => e.Id);
    }
}