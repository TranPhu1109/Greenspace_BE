using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.RoleId);
            builder.Property(e => e.RoleId).ValueGeneratedNever();
            builder.Property(e => e.RoleName);
        }
    }
}
