using System;
using System.Collections.Generic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public partial class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> entity)
    {
        entity.HasKey(e => e.Idemployee).HasName("PRIMARY");

        entity.ToTable("Employee");

        entity.HasIndex(e => e.Idstate, "Idstate");

        entity.Property(e => e.Eaddress).HasMaxLength(60);
        entity.Property(e => e.Ecity).HasMaxLength(60);
        entity.Property(e => e.Eemail).HasMaxLength(100);
        entity.Property(e => e.Elastname).HasMaxLength(50);
        entity.Property(e => e.Ename).HasMaxLength(50);
        entity.Property(e => e.Ephone).HasMaxLength(10);
        entity.Property(e => e.Eprint).HasMaxLength(50);

        entity.HasOne(d => d.IdstateNavigation).WithMany(p => p.Employees)
            .HasForeignKey(d => d.Idstate)
            .HasConstraintName("Employee_ibfk_1");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Employee> modelBuilder);
}
