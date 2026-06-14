using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class EmailHistoryConfiguration : IEntityTypeConfiguration<EmailHistory>
{
    public void Configure(EntityTypeBuilder<EmailHistory> builder)
    {
        builder.ToTable("EmailHistory");

        builder.HasKey(x => x.Idemailhistory);

        builder.Property(x => x.Entitytype)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Documentnumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Emailto)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Subject)
            .HasMaxLength(255);

        builder.Property(x => x.Datesent)
            .IsRequired();

        builder.HasOne(x => x.IdusersNavigation)
            .WithMany()
            .HasForeignKey(x => x.Idusers)
            .OnDelete(DeleteBehavior.Restrict);
    }
}