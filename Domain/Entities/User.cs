using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class User
{
    public int Idusers { get; set; }

    public string Ucustomer { get; set; } = null!;

    public string? Ubalance { get; set; }

    public DateOnly? Udate { get; set; }

    public int? Idaddress { get; set; }

    public int? Idterms { get; set; }

    public int? Idcustomer { get; set; }

    public int? Idrepsales { get; set; }

    public int? Idjobinfo { get; set; }

    public virtual ICollection<Estimate> Estimates { get; set; } = new List<Estimate>();

    public virtual Address? IdaddressNavigation { get; set; }

    public virtual Customer? IdcustomerNavigation { get; set; }

    public virtual Jobinfo? IdjobinfoNavigation { get; set; }

    public virtual Repsale? IdrepsalesNavigation { get; set; }

    public virtual Term? IdtermsNavigation { get; set; }
}
