using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Dapper.SqlMapper;

namespace GreenSpace.Infrastructure.FluentAPIs;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(e => e.ContractId);
        builder.Property(e => e.ContractId).ValueGeneratedNever();
        builder.HasOne(d => d.User).WithMany(p => p.Contracts)
            .HasForeignKey(d => d.UserId);
    }
}
