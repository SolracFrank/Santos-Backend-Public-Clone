using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class EstimateConfiguration : IEntityTypeConfiguration<Estimate>
{
    public void Configure(EntityTypeBuilder<Estimate> entity)
    {
        entity.HasKey(e => e.Idestimate).HasName("PRIMARY");

        entity.ToTable("Estimate");

        entity.HasIndex(e => e.CopiedFromEstimateId, "FK_Estimate_CopiedFrom");

        entity.HasIndex(e => e.Idcustomermessage, "FK_Estimate_Customermessage");

        entity.HasIndex(e => e.Idrepsales, "FK_Estimate_Repsales");

        entity.HasIndex(e => e.Idterms, "FK_Estimate_Terms");

        entity.HasIndex(e => e.Idusers, "FK_Estimate_Users");

        entity.HasIndex(e => e.Estimatenumber, "uq_Estimate_Estimatenumber").IsUnique();

        entity.Property(e => e.Activeestimate)
            .IsRequired()
            .HasDefaultValueSql("'1'");
        entity.Property(e => e.Customermessagetext).HasColumnType("text");
        entity.Property(e => e.Dateordered).HasColumnType("datetime");
        entity.Property(e => e.Datetobecompleted).HasColumnType("datetime");
        entity.Property(e => e.Estimatedate).HasColumnType("datetime");
        entity.Property(e => e.Expdate).HasColumnType("datetime");
        entity.Property(e => e.Markup).HasPrecision(10, 2);
        entity.Property(e => e.Memo).HasColumnType("text");
        entity.Property(e => e.Openbalance).HasPrecision(10, 2);
        entity.Property(e => e.Subtotal).HasPrecision(10, 2);
        entity.Property(e => e.Total).HasPrecision(10, 2);

        entity.HasOne(d => d.CopiedFromEstimate).WithMany(p => p.InverseCopiedFromEstimate)
            .HasForeignKey(d => d.CopiedFromEstimateId)
            .HasConstraintName("FK_Estimate_CopiedFrom");

        entity.HasOne(d => d.IdcustomermessageNavigation).WithMany(p => p.Estimates)
            .HasForeignKey(d => d.Idcustomermessage)
            .HasConstraintName("FK_Estimate_Customermessage");

        entity.HasOne(d => d.IdrepsalesNavigation).WithMany(p => p.Estimates)
            .HasForeignKey(d => d.Idrepsales)
            .HasConstraintName("FK_Estimate_Repsales");

        entity.HasOne(d => d.IdtermsNavigation).WithMany(p => p.Estimates)
            .HasForeignKey(d => d.Idterms)
            .HasConstraintName("FK_Estimate_Terms");

        entity.HasOne(d => d.IdusersNavigation).WithMany(p => p.Estimates)
            .HasForeignKey(d => d.Idusers)
            .HasConstraintName("FK_Estimate_Users");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Estimate> modelBuilder);
}
