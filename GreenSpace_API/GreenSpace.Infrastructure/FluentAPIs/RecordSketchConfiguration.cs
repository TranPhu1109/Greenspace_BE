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
    public class RecordSketchConfiguration : IEntityTypeConfiguration<RecordSketch>
    {
        public void Configure(EntityTypeBuilder<RecordSketch> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(x => x.ServiceOrder).WithMany(s => s.RecordSketches).HasForeignKey(rd => rd.ServiceOrderId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Image).WithMany().HasForeignKey(rd => rd.ImageId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
