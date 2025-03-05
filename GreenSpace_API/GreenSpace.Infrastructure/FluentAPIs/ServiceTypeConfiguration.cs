using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
{
    public void Configure(EntityTypeBuilder<ServiceType> builder)
    {
        builder.HasKey(e => e.ServiceTypeId);
        builder.Property(e => e.ServiceTypeId).ValueGeneratedNever();
        builder.Property(e => e.Name).HasMaxLength(100);
    }
}
