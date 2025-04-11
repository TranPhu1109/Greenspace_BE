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
    public class ComplaintDetailConfiguration : IEntityTypeConfiguration<ComplaintDetail>
    {
        public void Configure(EntityTypeBuilder<ComplaintDetail> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(d => d.Complaint).WithMany(p => p.ComplaintDetails).HasForeignKey(d => d.ComplaintId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(d => d.Product).WithMany(p => p.ComplaintDetails).HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
