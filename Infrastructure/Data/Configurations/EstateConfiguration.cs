using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class EstateConfiguration : IEntityTypeConfiguration<Estate>
{
    public void Configure(EntityTypeBuilder<Estate> entity)
    {
        entity.HasKey(e => e.Idestate).HasName("PRIMARY");

        entity.ToTable("Estate");

        entity.HasIndex(e => e.Esname, "Esname").IsUnique();

        entity.Property(e => e.Esname).HasMaxLength(100);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Estate> modelBuilder);
}
