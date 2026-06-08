using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> entity)
    {
        entity.HasKey(e => e.Idcustom).HasName("PRIMARY");

        entity.ToTable("Customer");

        entity.HasIndex(e => e.Cname, "Cname").IsUnique();

        entity.HasIndex(e => e.Idcustomfk, "Idcustomfk");

        entity.Property(e => e.Cname).HasMaxLength(50);

        entity.HasOne(d => d.IdcustomfkNavigation).WithMany(p => p.InverseIdcustomfkNavigation)
            .HasForeignKey(d => d.Idcustomfk)
            .HasConstraintName("Customer_ibfk_1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Customer> modelBuilder);
}
