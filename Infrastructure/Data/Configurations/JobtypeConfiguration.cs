using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class JobtypeConfiguration : IEntityTypeConfiguration<Jobtype>
{
    public void Configure(EntityTypeBuilder<Jobtype> entity)
    {
        entity.HasKey(e => e.Idjobtype).HasName("PRIMARY");

        entity.ToTable("Jobtype");

        entity.HasIndex(e => e.Idjobfk, "Idjobfk");

        entity.HasIndex(e => e.Jtname, "Jtname").IsUnique();

        entity.Property(e => e.Jtname).HasMaxLength(50);

        entity.HasOne(d => d.IdjobfkNavigation).WithMany(p => p.InverseIdjobfkNavigation)
            .HasForeignKey(d => d.Idjobfk)
            .HasConstraintName("Jobtype_ibfk_1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Jobtype> modelBuilder);
}
