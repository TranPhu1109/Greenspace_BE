using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Dapper.SqlMapper;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class UserWalletConfiguration : IEntityTypeConfiguration<UsersWallet>
{
    public void Configure(EntityTypeBuilder<UsersWallet> builder)
    {
        builder.HasKey(e => e.UserId);
        builder.Property(e => e.UserId).ValueGeneratedNever();
        builder.Property(e => e.WalletAccount).HasMaxLength(100);
        builder.HasOne(d => d.User).WithOne(p => p.UsersWallet).HasForeignKey<UsersWallet>(d => d.UserId);
    }
}
