using EsApi.Models;
using Microsoft.EntityFrameworkCore;
namespace EsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Terms> Terms { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Estate> Estates { get; set; }
    public DbSet<Employee> Employees { get; set;}
    public DbSet<Othername> Othernames { get; set; }
    public DbSet<Repsales> Repsales { get; set; }
    public DbSet<Jobtype> Jobtypes { get; set; }
    public DbSet<Jobinfo> Jobinfos { get; set; }
    public DbSet<Itemtype> Itemtypes { get; set; }
    public DbSet<Accounttype> Accounttypes { get; set; }
    public DbSet<Tax> Taxes { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Itservice> Itservices { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<Payt> Paytypes { get; set; }
    public DbSet<Paymd> Paymds { get; set; }
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Employee>().ToTable("Employee");
    modelBuilder.Entity<Account>().ToTable("Account");
    modelBuilder.Entity<Accounttype>().ToTable("Accounttype");
    modelBuilder.Entity<Address>().ToTable("Address");
    modelBuilder.Entity<Customer>().ToTable("Customer");
    modelBuilder.Entity<Estate>().ToTable("Estate");
    modelBuilder.Entity<Itemtype>().ToTable("Itemtype");
    modelBuilder.Entity<Itservice>().ToTable("Itservice");
    modelBuilder.Entity<Jobinfo>().ToTable("Jobinfo");
    modelBuilder.Entity<Jobtype>().ToTable("Jobtype");
    modelBuilder.Entity<Othername>().ToTable("Othername");
    modelBuilder.Entity<Repsales>().ToTable("Repsales");
    modelBuilder.Entity<Tax>().ToTable("Tax");
    modelBuilder.Entity<Terms>().ToTable("Terms");
    modelBuilder.Entity<Users>().ToTable("Users");
    modelBuilder.Entity<Payt>().ToTable("Payt");
    modelBuilder.Entity<Paymd>().ToTable("Paymd");

    modelBuilder.Entity<Address>().HasKey(x => x.Idaddress);
    modelBuilder.Entity<Terms>().HasKey(x => x.Idterms);
    modelBuilder.Entity<Customer>().HasKey(x => x.Idcustom);
    modelBuilder.Entity<Estate>().HasKey(x => x.Idestate);
    modelBuilder.Entity<Employee>().HasKey(x => x.Idemployee);
    modelBuilder.Entity<Othername>().HasKey(x => x.Idother);
    modelBuilder.Entity<Repsales>().HasKey(x => x.Idrepsales);
    modelBuilder.Entity<Jobtype>().HasKey(x => x.Idjobtype);
    modelBuilder.Entity<Jobinfo>().HasKey(x => x.Idjobinfo);
    modelBuilder.Entity<Itemtype>().HasKey(x => x.Iditemt);
    modelBuilder.Entity<Accounttype>().HasKey(x => x.Idaccountt);
    modelBuilder.Entity<Tax>().HasKey(x => x.Idtax);
    modelBuilder.Entity<Account>().HasKey(x => x.Idaccount);
    modelBuilder.Entity<Itservice>().HasKey(x => x.Iditem);
    modelBuilder.Entity<Users>().HasKey(x => x.Idusers);
    modelBuilder.Entity<Payt>().HasKey(x => x.Idpayt);
    modelBuilder.Entity<Paymd>().HasKey(x => x.Idpaymd);
    base.OnModelCreating(modelBuilder);
}
}