using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class ServiceFeedbackConfiguration : IEntityTypeConfiguration<ServiceFeedback>
    {
        public void Configure(EntityTypeBuilder<ServiceFeedback> builder)
        {
            builder.HasKey(e => new { e.UserId, e.DesignIdeaId });
            builder.Property(e => e.Description);
            builder.HasOne(d => d.DesignIdea).WithMany(p => p.ServiceFeedbacks).HasForeignKey(d => d.DesignIdeaId);
            builder.HasOne(d => d.User).WithMany(p => p.ServiceFeedbacks).HasForeignKey(d => d.UserId);
        }
    }
}
