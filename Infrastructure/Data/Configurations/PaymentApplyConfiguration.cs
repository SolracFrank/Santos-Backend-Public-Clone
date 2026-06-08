using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class PaymentApplyConfiguration : IEntityTypeConfiguration<PaymentApply>
{
    public void Configure(EntityTypeBuilder<PaymentApply> entity)
    {
        entity.HasKey(e => e.Idpaymentapply).HasName("PRIMARY");

        entity.HasIndex(e => e.Idinvoice, "IX_PaymentApplies_Invoice");

        entity.HasIndex(e => e.Idpayment, "IX_PaymentApplies_Payment");

        entity.Property(e => e.AmountApplied).HasPrecision(18, 2);

        entity.HasOne(d => d.IdinvoiceNavigation).WithMany(p => p.PaymentApplies)
            .HasForeignKey(d => d.Idinvoice)
            .HasConstraintName("FK_PaymentApplies_Invoice");

        entity.HasOne(d => d.IdpaymentNavigation).WithMany(p => p.PaymentApplies)
            .HasForeignKey(d => d.Idpayment)
            .HasConstraintName("FK_PaymentApplies_Payment");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<PaymentApply> modelBuilder);
}
