using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class WebManagerConfiguration : IEntityTypeConfiguration<WebManager>
{
    public void Configure(EntityTypeBuilder<WebManager> builder)
    {
        builder.HasNoKey();
    }
}
