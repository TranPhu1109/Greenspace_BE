using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class TasksConfiguration : IEntityTypeConfiguration<WorkTask>
{
    public void Configure(EntityTypeBuilder<WorkTask> builder)
    {
        builder.HasKey(e => new { e.ServiceOrderId, e.UserId });
        builder.HasOne(d => d.ServiceOrder).WithMany(p => p.WorkTask).HasForeignKey(d => d.ServiceOrderId);
        builder.HasOne(d => d.User).WithMany(p => p.WorkTask).HasForeignKey(d => d.UserId);
    }
}
