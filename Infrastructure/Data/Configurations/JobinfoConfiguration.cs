using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class JobinfoConfiguration : IEntityTypeConfiguration<Jobinfo>
{
    public void Configure(EntityTypeBuilder<Jobinfo> entity)
    {
        entity.HasKey(e => e.Idjobinfo).HasName("PRIMARY");

        entity.ToTable("Jobinfo");

        entity.HasIndex(e => e.Idjobtype, "Idjobtype");

        entity.Property(e => e.Jdescription).HasMaxLength(150);
        entity.Property(e => e.Jjobstatus).HasMaxLength(50);

        entity.HasOne(d => d.IdjobtypeNavigation).WithMany(p => p.Jobinfos)
            .HasForeignKey(d => d.Idjobtype)
            .HasConstraintName("Jobinfo_ibfk_1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Jobinfo> modelBuilder);
}
