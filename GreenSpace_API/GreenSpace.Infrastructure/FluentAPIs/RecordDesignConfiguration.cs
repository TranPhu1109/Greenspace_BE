using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class RecordDesignConfiguration : IEntityTypeConfiguration<RecordDesign>
    {
        public void Configure(EntityTypeBuilder<RecordDesign> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(x => x.ServiceOrder).WithMany(s => s.RecordDesigns).HasForeignKey(rd => rd.ServiceOrderId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Image).WithMany().HasForeignKey(rd => rd.ImageId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
