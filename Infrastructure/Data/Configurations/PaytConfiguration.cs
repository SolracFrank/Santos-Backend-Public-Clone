using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class PaytConfiguration : IEntityTypeConfiguration<Payt>
{
    public void Configure(EntityTypeBuilder<Payt> entity)
    {
        entity.HasKey(e => e.Idpayt).HasName("PRIMARY");

        entity.ToTable("Payt");

        entity.HasIndex(e => e.Ptname, "Ptname").IsUnique();

        entity.Property(e => e.Ptname).HasMaxLength(30);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Payt> modelBuilder);
}
