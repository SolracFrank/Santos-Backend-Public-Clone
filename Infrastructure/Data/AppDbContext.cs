using Domain.Entities;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Accounttype> Accounttypes { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customermessage> Customermessages { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Estate> Estates { get; set; }

    public virtual DbSet<Estimate> Estimates { get; set; }

    public virtual DbSet<Estimatedetail> Estimatedetails { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Invoicedetail> Invoicedetails { get; set; }

    public virtual DbSet<Itemtype> Itemtypes { get; set; }

    public virtual DbSet<Itservice> Itservices { get; set; }

    public virtual DbSet<Jobinfo> Jobinfos { get; set; }

    public virtual DbSet<Jobtype> Jobtypes { get; set; }

    public virtual DbSet<Othername> Othernames { get; set; }

    public virtual DbSet<Paymd> Paymds { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentApply> PaymentApplies { get; set; }

    public virtual DbSet<Payt> Payts { get; set; }

    public virtual DbSet<Repsale> Repsales { get; set; }

    public virtual DbSet<Tax> Taxes { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<TaxRate> TaxRates { get; set; }

    public virtual DbSet<EmailHistory> EmailHistories { get; set; }

    public DbSet<LoginUser> LoginUsers { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmailHistoryConfiguration());

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Idauditlog);

            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.EntityName)
                .HasMaxLength(100);

            entity.Property(e => e.EntityId)
                .HasMaxLength(100);

            entity.Property(e => e.Username)
                .HasMaxLength(100);

            entity.Property(e => e.IpAddress)
                .HasMaxLength(100);

            entity.Property(e => e.PerformedByRole)
                .HasMaxLength(50);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Severity)
                .HasConversion<string>()   
                .HasMaxLength(20);

            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });


        modelBuilder.Entity<LoginUser>(entity =>
        {
            entity.Property(e => e.UserRole)
            .HasMaxLength(50)
            .IsRequired();

            entity.Property(e => e.SecurityStamp)
            .HasMaxLength(36)
            .IsRequired();
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
