using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class ItemtypeConfiguration : IEntityTypeConfiguration<Itemtype>
{
    public void Configure(EntityTypeBuilder<Itemtype> entity)
    {
        entity.HasKey(e => e.Iditemt).HasName("PRIMARY");

        entity.ToTable("Itemtype");

        entity.HasIndex(e => e.Itname, "Itname").IsUnique();

        entity.Property(e => e.Itname).HasMaxLength(50);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Itemtype> modelBuilder);
}
