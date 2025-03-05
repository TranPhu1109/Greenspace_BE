using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GreenSpace.Infrastructure.FluentAPIs
{
    public class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(e => e.DocumentId);
            builder.Property(e => e.DocumentId).ValueGeneratedNever();
            builder.Property(e => e.Document1);
        }
    }
}
