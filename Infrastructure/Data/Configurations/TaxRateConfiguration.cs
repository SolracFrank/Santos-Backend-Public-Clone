using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class TaxRateConfiguration : IEntityTypeConfiguration<TaxRate>
{
    public void Configure(EntityTypeBuilder<TaxRate> entity)
    {
        entity.HasKey(e => e.Idtaxrate).HasName("PRIMARY");

        entity.HasIndex(e => e.Name, "UQ_TaxRates_Name").IsUnique();

        entity.Property(e => e.Name)
            .HasMaxLength(150)
            .IsRequired();

        entity.Property(e => e.Rate)
            .HasPrecision(10, 4)
            .HasDefaultValueSql("'0'");

        entity.Property(e => e.Active)
            .IsRequired()
            .HasDefaultValueSql("'1'");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<TaxRate> modelBuilder);
}