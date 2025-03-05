using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class RefeshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(e => new { e.UserId, e.Token });
        builder.Property(e => e.Token).HasMaxLength(255);
        builder.Property(e => e.ExpiredAt);
        builder.Property(e => e.IssuedAt);
        builder.Property(e => e.JwtId).HasMaxLength(255);
        builder.HasOne(d => d.User).WithMany(p => p.RefreshTokens).HasForeignKey(d => d.UserId);
    }
}
