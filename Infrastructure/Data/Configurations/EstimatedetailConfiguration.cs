using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class EstimatedetailConfiguration : IEntityTypeConfiguration<Estimatedetail>
{
    public void Configure(EntityTypeBuilder<Estimatedetail> entity)
    {
        entity.HasKey(e => e.Idestimatedetail).HasName("PRIMARY");

        entity.ToTable("Estimatedetail");

        entity.HasIndex(e => e.Idestimate, "FK_Estimatedetail_Estimate");

        entity.HasIndex(e => e.Iditem, "FK_Estimatedetail_Itservice");

        entity.Property(e => e.Cost).HasPrecision(10, 2);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.Itemname).HasMaxLength(150);
        entity.Property(e => e.Qty).HasPrecision(10, 2);
        entity.Property(e => e.Total).HasPrecision(10, 2);

        entity.HasOne(d => d.IdestimateNavigation).WithMany(p => p.Estimatedetails)
            .HasForeignKey(d => d.Idestimate)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Estimatedetail_Estimate");

        entity.HasOne(d => d.IditemNavigation).WithMany(p => p.Estimatedetails)
            .HasForeignKey(d => d.Iditem)
            .HasConstraintName("FK_Estimatedetail_Itservice");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Estimatedetail> modelBuilder);
}
