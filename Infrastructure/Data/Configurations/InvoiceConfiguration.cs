using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> entity)
    {
        entity.HasKey(e => e.Idinvoice).HasName("PRIMARY");

        entity.HasIndex(e => e.Invoicenumber, "uq_Invoices_Invoicenumber").IsUnique();

        entity.Property(e => e.Activeinvoice)
            .IsRequired()
            .HasDefaultValueSql("'1'");
        entity.Property(e => e.Customermessagetext).HasColumnType("text");
        entity.Property(e => e.Dateordered).HasColumnType("datetime");
        entity.Property(e => e.Dteordered).HasColumnType("datetime");
        entity.Property(e => e.Duedate).HasColumnType("datetime");
        entity.Property(e => e.Invoicedate).HasColumnType("datetime");
        entity.Property(e => e.Markup).HasPrecision(18, 2);
        entity.Property(e => e.Memo).HasColumnType("text");
        entity.Property(e => e.Openbalance).HasPrecision(18, 2);
        entity.Property(e => e.Ponumber).HasMaxLength(100);
        entity.Property(e => e.Subtotal).HasPrecision(18, 2);
        entity.Property(e => e.Total).HasPrecision(18, 2);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Invoice> modelBuilder);
}
