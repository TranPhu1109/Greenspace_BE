using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class TransactionPercentageConfiguration : IEntityTypeConfiguration<TransactionPercentage>
    {
        public void Configure(EntityTypeBuilder<TransactionPercentage> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
