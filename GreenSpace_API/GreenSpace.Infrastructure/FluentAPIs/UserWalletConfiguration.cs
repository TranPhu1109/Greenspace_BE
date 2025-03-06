using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Dapper.SqlMapper;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class UserWalletConfiguration : IEntityTypeConfiguration<UsersWallet>
{
    public void Configure(EntityTypeBuilder<UsersWallet> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(d => d.User).WithOne(p => p.UsersWallet).HasForeignKey<UsersWallet>(d => d.UserId);
    }
}
