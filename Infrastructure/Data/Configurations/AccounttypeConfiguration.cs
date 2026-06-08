using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class AccounttypeConfiguration : IEntityTypeConfiguration<Accounttype>
{
    public void Configure(EntityTypeBuilder<Accounttype> entity)
    {
        entity.HasKey(e => e.Idaccountt).HasName("PRIMARY");

        entity.ToTable("Accounttype");

        entity.HasIndex(e => e.Atype, "Atype").IsUnique();

        entity.Property(e => e.Atype).HasMaxLength(50);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Accounttype> modelBuilder);
}
