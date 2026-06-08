using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class OthernameConfiguration : IEntityTypeConfiguration<Othername>
{
    public void Configure(EntityTypeBuilder<Othername> entity)
    {
        entity.HasKey(e => e.Idother).HasName("PRIMARY");

        entity.ToTable("Othername");

        entity.HasIndex(e => e.Idstate, "Idstate");

        entity.Property(e => e.Oaddress).HasMaxLength(60);
        entity.Property(e => e.Ocity).HasMaxLength(60);
        entity.Property(e => e.Oemail).HasMaxLength(120);
        entity.Property(e => e.Olastname).HasMaxLength(50);
        entity.Property(e => e.Oname).HasMaxLength(50);
        entity.Property(e => e.Ophone).HasMaxLength(10);

        entity.HasOne(d => d.IdstateNavigation).WithMany(p => p.Othernames)
            .HasForeignKey(d => d.Idstate)
            .HasConstraintName("Othername_ibfk_1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Othername> modelBuilder);
}
