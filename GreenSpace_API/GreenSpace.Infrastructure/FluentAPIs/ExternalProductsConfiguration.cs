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
    public class ExternalProductsConfiguration :  IEntityTypeConfiguration<ExternalProducts>
    {
        public void Configure(EntityTypeBuilder<ExternalProducts> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(d => d.ServiceOrder).WithMany(p => p.ExternalProducts).HasForeignKey(d => d.ServiceOrderId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
