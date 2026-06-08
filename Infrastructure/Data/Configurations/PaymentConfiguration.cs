using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> entity)
    {
        entity.HasKey(e => e.Idpayment).HasName("PRIMARY");

        entity.HasIndex(e => e.Paymentnumber, "UQ_Payments_Number").IsUnique();

        entity.Property(e => e.Amount).HasPrecision(18, 2);
        entity.Property(e => e.Memo).HasColumnType("text");
        entity.Property(e => e.Paymentdate).HasColumnType("datetime");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Payment> modelBuilder);
}
