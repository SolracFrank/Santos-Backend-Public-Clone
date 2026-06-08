using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> entity)
    {
        entity.HasKey(e => e.Idaddress).HasName("PRIMARY");

        entity.ToTable("Address");

        entity.Property(e => e.Accemail).HasMaxLength(120);
        entity.Property(e => e.Acompany).HasMaxLength(60);
        entity.Property(e => e.Aemail).HasMaxLength(120);
        entity.Property(e => e.Afirstname).HasMaxLength(50);
        entity.Property(e => e.Ainvoice).HasMaxLength(200);
        entity.Property(e => e.Ajobtitle).HasMaxLength(50);
        entity.Property(e => e.Alastname).HasMaxLength(50);
        entity.Property(e => e.Amainphone).HasMaxLength(10);
        entity.Property(e => e.Aother).HasMaxLength(120);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Address> modelBuilder);
}
