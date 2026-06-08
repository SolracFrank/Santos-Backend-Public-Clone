using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class InvoicedetailConfiguration : IEntityTypeConfiguration<Invoicedetail>
{
    public void Configure(EntityTypeBuilder<Invoicedetail> entity)
    {
        entity.HasKey(e => e.Idinvoicedetail).HasName("PRIMARY");

        entity.HasIndex(e => e.Idinvoice, "ix_Invoicedetails_Idinvoice");

        entity.Property(e => e.Cost).HasPrecision(18, 2);
        entity.Property(e => e.Description).HasColumnType("text");
        entity.Property(e => e.Itemname).HasMaxLength(255);
        entity.Property(e => e.Qty).HasPrecision(18, 2);
        entity.Property(e => e.Total).HasPrecision(18, 2);

        entity.HasOne(d => d.IdinvoiceNavigation).WithMany(p => p.Invoicedetails)
            .HasForeignKey(d => d.Idinvoice)
            .HasConstraintName("fk_Invoicedetails_Invoices");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Invoicedetail> modelBuilder);
}
