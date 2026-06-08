using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class PaymdConfiguration : IEntityTypeConfiguration<Paymd>
{
    public void Configure(EntityTypeBuilder<Paymd> entity)
    {
        entity.HasKey(e => e.Idpaymd).HasName("PRIMARY");

        entity.ToTable("Paymd");

        entity.HasIndex(e => e.Idpayt, "Idpayt");

        entity.HasIndex(e => e.Pmdname, "Pmdname").IsUnique();

        entity.Property(e => e.Pmdname).HasMaxLength(80);

        entity.HasOne(d => d.IdpaytNavigation).WithMany(p => p.Paymds)
            .HasForeignKey(d => d.Idpayt)
            .HasConstraintName("Paymd_ibfk_1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Paymd> modelBuilder);
}
