using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.Idusers).HasName("PRIMARY");

        entity.HasIndex(e => e.Idaddress, "Idaddress");

        entity.HasIndex(e => e.Idcustomer, "Idcustomer");

        entity.HasIndex(e => e.Idjobinfo, "Idjobinfo");

        entity.HasIndex(e => e.Idrepsales, "Idrepsales");

        entity.HasIndex(e => e.Idterms, "Idterms");

        entity.Property(e => e.Ubalance).HasMaxLength(50);
        entity.Property(e => e.Ucustomer).HasMaxLength(100);

        entity.HasOne(d => d.IdaddressNavigation).WithMany(p => p.Users)
            .HasForeignKey(d => d.Idaddress)
            .HasConstraintName("Users_ibfk_1");

        entity.HasOne(d => d.IdcustomerNavigation).WithMany(p => p.Users)
            .HasForeignKey(d => d.Idcustomer)
            .HasConstraintName("Users_ibfk_3");

        entity.HasOne(d => d.IdjobinfoNavigation).WithMany(p => p.Users)
            .HasForeignKey(d => d.Idjobinfo)
            .HasConstraintName("Users_ibfk_5");

        entity.HasOne(d => d.IdrepsalesNavigation).WithMany(p => p.Users)
            .HasForeignKey(d => d.Idrepsales)
            .HasConstraintName("Users_ibfk_4");

        entity.HasOne(d => d.IdtermsNavigation).WithMany(p => p.Users)
            .HasForeignKey(d => d.Idterms)
            .HasConstraintName("Users_ibfk_2");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<User> modelBuilder);
}
