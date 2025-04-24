using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class ComplaintReasonConfiguration : IEntityTypeConfiguration<ComplaintReason>
    {
        public void Configure(EntityTypeBuilder<ComplaintReason> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
