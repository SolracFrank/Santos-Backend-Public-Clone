using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EsApi.Models;

public class Address
{
    [Key] public int Idaddress { get; set; }
    public string? Acompany { get; set; }
    public string Afirstname { get; set; } = null!;
    public string Alastname { get; set; } = null!;
    public string? Ajobtitle { get; set; } 
    public string? Amainphone { get; set; }
    public string? Aemail { get; set; } 
    public string? Aother { get; set; } 
    public string? Accemail { get; set; } 
    public string? Ainvoice { get; set; } 
}

public class Terms
{
    [Key] public int Idterms { get; set; }
    public string Tname { get; set; } = null!;
    public int? Tnets { get; set; }
    public decimal? Tperccs { get; set; }
    public int? Tnetd { get; set; }
    public int? Tdags { get; set; }
    public decimal? Tpercd { get; set; }
}

public class Customer
{
    [Key] public int Idcustom { get; set; }
    public string? Cname { get; set; }
    public int? Idcustomfk { get; set; }
}

public class Estate
{
    [Key] public int Idestate { get; set; }
    public string? Esname { get; set; }
}

public class Employee
{
    [Key]
    public int Idemployee { get; set; }
    public string Ename { get; set; } = null!;
    public string Elastname { get; set; } = null!;
    public string? Eaddress { get; set; }
    public string? Ecity { get; set; }
    public int? Ezip { get; set; }
    public string? Eprint { get; set; }
    public string? Eemail { get; set; }
    public string? Ephone { get; set; }
    public int? Idstate { get; set; }

    [ForeignKey(nameof(Idstate))]
    public Estate? Estate { get; set; }
}

public class Othername
{
    [Key] public int Idother { get; set; }
    public string Oname { get; set; } = null!;
    public string Olastname { get; set; } = null!;
    public string? Oaddress { get; set; } 
    public string? Ocity { get; set; } 
    public int? Ozip { get; set; }
    public string? Ophone { get; set; } = null!;
    public string? Oemail { get; set; } = null!;

    public int? Idstate { get; set; }

    [ForeignKey(nameof(Idstate))]
    public Estate? Estate { get; set; }
}

public class Repsales
{
    [Key] public int Idrepsales { get; set; }
    public string Rinitials { get; set; } = null!;
    public int? Idemployee { get; set; }

    [ForeignKey(nameof(Idemployee))]
    public Employee? Employee { get; set; }

    public int? Idothern { get; set; }

    [ForeignKey(nameof(Idothern))]
    public Othername? Othername { get; set; }
}

public class Jobtype
{
    [Key] public int Idjobtype { get; set; }
    public string Jtname { get; set; } = null!;
    public int? Idjobfk { get; set; }
}

public class Jobinfo
{
    [Key] public int Idjobinfo { get; set; }
    public string Jdescription { get; set; } = null!;
    public string Jjobstatus { get; set; } = null!;

    public int? Idjobtype { get; set; }

    [ForeignKey(nameof(Idjobtype))]
    public Jobtype? Jobtype { get; set; }
}

public class Itemtype
{
    [Key] public int Iditemt { get; set; }
    public string Itname { get; set; } = null!;
}

public class Accounttype
{
    [Key] public int Idaccountt { get; set; }
    public string Atype { get; set; } = null!;
}

public class Tax
{
    [Key] public int Idtax { get; set; }
    public string Tname { get; set; } = null!;
}

public class Payt
{
    [Key] public int Idpayt { get; set; }
    public string Ptname { get; set; } = null!;
}

public class Paymd
{
    [Key] public int Idpaymd { get; set; }
    public string Pmdname { get; set; } = null!;
     public int? Idpayt { get; set; }

    [ForeignKey(nameof(Idpayt))]
    public Payt? Payt    { get; set; }
}

public class Account
{
    [Key] public int Idaccount { get; set; }
    public string Aname { get; set; } = null!;
    public string? Adescription { get; set; }
    public string? Abankno { get; set; } 
    public string? Anotes { get; set; } 

    public int? Idtax { get; set; }

    [ForeignKey(nameof(Idtax))]
    public Tax? Tax { get; set; }

    public int? Idaccountt { get; set; }

    [ForeignKey(nameof(Idaccountt))]
    public Accounttype? Accounttype { get; set; }
}

public class Itservice
{
    [Key] public int Iditem { get; set; }
    public string Iname { get; set; } = null!;
    public string? Idescription { get; set; }
    public string? Irate { get; set; } 
    public string? Idescpurchase { get; set; }
    public string? Icost { get; set; }
    public string? Idescsales { get; set; }
    public string? Isales { get; set; }

    public int? Iditemt { get; set; }

    [ForeignKey(nameof(Iditemt))]
    public Itemtype? Itemtype { get; set; }

    public int? Idaccountp { get; set; }

    [ForeignKey(nameof(Idaccountp))]
    public Account? Accountp { get; set; }

    public int? Idaccounts { get; set; }

    [ForeignKey(nameof(Idaccounts))]
    public Account? Accounts { get; set; }

    public int? Idaccountn { get; set; }

    [ForeignKey(nameof(Idaccountn))]
    public Account? Accountn { get; set; }

    public int? Idpaymd { get; set; }

    [ForeignKey(nameof(Idpaymd))]
     public Paymd? Paymd { get; set; }
     
     public int? Iditemfk { get; set; }
}

public class Users
{
    [Key] public int Idusers { get; set; }
    public string Ucustomer { get; set; } = null!;
    public DateTime Udate { get; set; }
    public string Ubalance { get; set; } = null!;

    public int? Idaddress { get; set; }

    [ForeignKey(nameof(Idaddress))]
    public Address? Address { get; set; }

    public int? Idterms { get; set; }

    [ForeignKey(nameof(Idterms))]
    public Terms? Terms { get; set; }

    public int? Idcustomer { get; set; }

    [ForeignKey(nameof(Idcustomer))]
    public Customer? Customer { get; set; }

    public int? Idrepsales { get; set; }

    [ForeignKey(nameof(Idrepsales))]
    public Repsales? Repsales { get; set; }

    public int? Idjobinfo { get; set; }

    [ForeignKey(nameof(Idjobinfo))]
    public Jobinfo? Jobinfo { get; set; }
}

 public class Estimatedetail
 {
        [Key]
        public int Idestimatedetail { get; set; }
        public string? Itemname { get; set; }
        public string? Description { get; set; }
        public decimal Qty { get; set; }
        public decimal Cost { get; set; }
        public decimal Total { get; set; }
        public int Idestimate { get; set; }

        [ForeignKey(nameof(Idestimate))]
        public Estimate? Estimate { get; set; }
        public int? Iditem { get; set; }

        [ForeignKey(nameof(Iditem))]
        public Itservice? Itservice { get; set; }
    }

    public class Customermessage
    {
        [Key]
        public int Idcustomermessage { get; set; }

        public string Message { get; set; } = null!;
    }
public class Estimate
{
    [Key]
    public int Idestimate { get; set; }

    public int Estimatenumber { get; set; }
    public DateTime Estimatedate { get; set; }
    public DateTime? Expdate { get; set; }
    public DateTime? Datetobecompleted { get; set; }
    public DateTime? Dateordered { get; set; }

    public string? Memo { get; set; }

    public decimal Subtotal { get; set; }
    public decimal Markup { get; set; }
    public decimal Total { get; set; }
    public bool Activeestimate { get; set; } = true;
    public decimal Openbalance { get; set; }

    public int? Idusers { get; set; }

    [ForeignKey(nameof(Idusers))]
    public Users? Users { get; set; }

    public int? Idterms { get; set; }

    [ForeignKey(nameof(Idterms))]
    public Terms? Terms { get; set; }

    public int? Idrepsales { get; set; }

    [ForeignKey(nameof(Idrepsales))]
    public Repsales? Repsales { get; set; }

    public int? Idcustomermessage { get; set; }

    [ForeignKey(nameof(Idcustomermessage))]
    public Customermessage? Customermessage { get; set; }

    public string? Customermessagetext { get; set; }

    public ICollection<Estimatedetail>? Estimatedetails { get; set; }
    public bool IsCopy { get; set; }

    public int? Idestimatefk { get; set; }
}

