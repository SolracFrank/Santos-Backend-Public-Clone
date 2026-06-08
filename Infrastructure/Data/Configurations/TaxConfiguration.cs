using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class TaxConfiguration : IEntityTypeConfiguration<Tax>
{
    public void Configure(EntityTypeBuilder<Tax> entity)
    {
        entity.HasKey(e => e.Idtax).HasName("PRIMARY");

        entity.ToTable("Tax");

        entity.HasIndex(e => e.Tname, "Tname").IsUnique();

        entity.Property(e => e.Tname).HasMaxLength(150);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Tax> modelBuilder);
}
