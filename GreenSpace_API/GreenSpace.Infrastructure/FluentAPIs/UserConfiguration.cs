using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);;
        builder.HasOne(x => x.UsersWallet).WithOne(x => x.User).HasForeignKey<UsersWallet>(x => x.UserId);
        builder.HasMany(x => x.Orders).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        builder.HasMany(x => x.ServiceFeedbacks).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        builder.HasMany(x => x.ServiceOrders).WithOne(x => x.User).HasForeignKey(x => x.UserId);
    }
}