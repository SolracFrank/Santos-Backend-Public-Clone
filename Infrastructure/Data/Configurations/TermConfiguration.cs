using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class TermConfiguration : IEntityTypeConfiguration<Term>
{
    public void Configure(EntityTypeBuilder<Term> entity)
    {
        entity.HasKey(e => e.Idterms).HasName("PRIMARY");

        entity.HasIndex(e => e.Tname, "Tname").IsUnique();

        entity.Property(e => e.Tname).HasMaxLength(100);
        entity.Property(e => e.Tperccs).HasPrecision(3, 2);
        entity.Property(e => e.Tpercd).HasPrecision(3, 2);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Term> modelBuilder);
}
