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
    public class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.ServiceOrder).WithMany().HasForeignKey(x => x.ServiceOrderId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Image).WithMany().HasForeignKey(x => x.ImageId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
