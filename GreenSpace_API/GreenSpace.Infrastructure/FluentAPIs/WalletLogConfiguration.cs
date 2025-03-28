using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class WalletLogConfiguration : IEntityTypeConfiguration<WalletLog>
    {
        public void Configure(EntityTypeBuilder<WalletLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.UsersWallet).WithMany(x => x.WalletLogs).HasForeignKey(x => x.WalletId);
        }
    }
}
