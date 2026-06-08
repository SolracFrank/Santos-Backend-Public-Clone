using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> entity)
    {
        entity.HasKey(e => e.Idaccount).HasName("PRIMARY");

        entity.ToTable("Account");

        entity.HasIndex(e => e.Aname, "Aname").IsUnique();

        entity.HasIndex(e => e.Idaccountt, "Idaccountt");

        entity.HasIndex(e => e.Idtax, "Idtax");

        entity.Property(e => e.Abankno).HasMaxLength(30);
        entity.Property(e => e.Adescription).HasMaxLength(300);
        entity.Property(e => e.Aname).HasMaxLength(60);
        entity.Property(e => e.Anotes).HasMaxLength(100);

        entity.HasOne(d => d.IdaccounttNavigation).WithMany(p => p.Accounts)
            .HasForeignKey(d => d.Idaccountt)
            .HasConstraintName("Account_ibfk_2");

        entity.HasOne(d => d.IdtaxNavigation).WithMany(p => p.Accounts)
            .HasForeignKey(d => d.Idtax)
            .HasConstraintName("Account_ibfk_1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Account> modelBuilder);
}
