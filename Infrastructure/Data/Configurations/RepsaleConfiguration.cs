using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class RepsaleConfiguration : IEntityTypeConfiguration<Repsale>
{
    public void Configure(EntityTypeBuilder<Repsale> entity)
    {
        entity.HasKey(e => e.Idrepsales).HasName("PRIMARY");

        entity.HasIndex(e => e.Idemployee, "Idemployee");

        entity.HasIndex(e => e.Idothern, "Idothern");

        entity.Property(e => e.Rinitials).HasMaxLength(10);

        entity.HasOne(d => d.IdemployeeNavigation).WithMany(p => p.Repsales)
            .HasForeignKey(d => d.Idemployee)
            .HasConstraintName("Repsales_ibfk_1");

        entity.HasOne(d => d.IdothernNavigation).WithMany(p => p.Repsales)
            .HasForeignKey(d => d.Idothern)
            .HasConstraintName("Repsales_ibfk_2");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Repsale> modelBuilder);
}
