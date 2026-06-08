using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class CustomermessageConfiguration : IEntityTypeConfiguration<Customermessage>
{
    public void Configure(EntityTypeBuilder<Customermessage> entity)
    {
        entity.HasKey(e => e.Idcustomermessage).HasName("PRIMARY");

        entity.ToTable("Customermessage");

        entity.Property(e => e.Message).HasColumnType("text");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Customermessage> modelBuilder);
}
