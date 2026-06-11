using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
