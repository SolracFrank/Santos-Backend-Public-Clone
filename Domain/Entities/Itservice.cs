using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Itservice
{
    public int Iditem { get; set; }

    public string Iname { get; set; } = null!;

    public string? Idescription { get; set; }

    public string? Irate { get; set; }

    public string? Idescpurchase { get; set; }

    public string? Icost { get; set; }

    public string? Idescsales { get; set; }

    public string? Isales { get; set; }

    public int? Iditemt { get; set; }

    public int? Idaccountp { get; set; }

    public int? Idaccounts { get; set; }

    public int? Idaccountn { get; set; }

    public int? Idpaymd { get; set; }

    public int? Iditemfk { get; set; }

    public virtual ICollection<Estimatedetail> Estimatedetails { get; set; } = new List<Estimatedetail>();

    public virtual Account? IdaccountnNavigation { get; set; }

    public virtual Account? IdaccountpNavigation { get; set; }

    public virtual Account? IdaccountsNavigation { get; set; }

    public virtual Itservice? IditemfkNavigation { get; set; }

    public virtual Itemtype? IditemtNavigation { get; set; }

    public virtual Paymd? IdpaymdNavigation { get; set; }

    public virtual ICollection<Itservice> InverseIditemfkNavigation { get; set; } = new List<Itservice>();
}
