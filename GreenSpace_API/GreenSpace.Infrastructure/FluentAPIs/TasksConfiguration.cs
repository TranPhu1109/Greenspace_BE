﻿using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class TasksConfiguration : IEntityTypeConfiguration<WorkTask>
{
    public void Configure(EntityTypeBuilder<WorkTask> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(d => d.ServiceOrder).WithMany(p => p.WorkTask).HasForeignKey(d => d.ServiceOrderId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(d => d.User).WithMany(p => p.WorkTask).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.Restrict);
    }
}
