using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class ItserviceConfiguration : IEntityTypeConfiguration<Itservice>
{
    public void Configure(EntityTypeBuilder<Itservice> entity)
    {
        entity.HasKey(e => e.Iditem).HasName("PRIMARY");

        entity.ToTable("Itservice");

        entity.HasIndex(e => e.Idpaymd, "FK_Itservice_Paymd");

        entity.HasIndex(e => e.Iditemfk, "Fk_Itservice_Itservice");

        entity.HasIndex(e => e.Idaccountn, "Idaccountn");

        entity.HasIndex(e => e.Idaccountp, "Idaccountp");

        entity.HasIndex(e => e.Idaccounts, "Idaccounts");

        entity.HasIndex(e => e.Iditemt, "Iditemt");

        entity.HasIndex(e => e.Iname, "Iname").IsUnique();

        entity.Property(e => e.Icost).HasMaxLength(50);
        entity.Property(e => e.Idescpurchase).HasMaxLength(300);
        entity.Property(e => e.Idescription).HasMaxLength(300);
        entity.Property(e => e.Idescsales).HasMaxLength(300);
        entity.Property(e => e.Iname).HasMaxLength(50);
        entity.Property(e => e.Irate).HasMaxLength(50);
        entity.Property(e => e.Isales).HasMaxLength(50);

        entity.HasOne(d => d.IdaccountnNavigation).WithMany(p => p.ItserviceIdaccountnNavigations)
            .HasForeignKey(d => d.Idaccountn)
            .HasConstraintName("Itservice_ibfk_4");

        entity.HasOne(d => d.IdaccountpNavigation).WithMany(p => p.ItserviceIdaccountpNavigations)
            .HasForeignKey(d => d.Idaccountp)
            .HasConstraintName("Itservice_ibfk_2");

        entity.HasOne(d => d.IdaccountsNavigation).WithMany(p => p.ItserviceIdaccountsNavigations)
            .HasForeignKey(d => d.Idaccounts)
            .HasConstraintName("Itservice_ibfk_3");

        entity.HasOne(d => d.IditemfkNavigation).WithMany(p => p.InverseIditemfkNavigation)
            .HasForeignKey(d => d.Iditemfk)
            .HasConstraintName("Fk_Itservice_Itservice");

        entity.HasOne(d => d.IditemtNavigation).WithMany(p => p.Itservices)
            .HasForeignKey(d => d.Iditemt)
            .HasConstraintName("Itservice_ibfk_1");

        entity.HasOne(d => d.IdpaymdNavigation).WithMany(p => p.Itservices)
            .HasForeignKey(d => d.Idpaymd)
            .HasConstraintName("FK_Itservice_Paymd");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Itservice> modelBuilder);
}
