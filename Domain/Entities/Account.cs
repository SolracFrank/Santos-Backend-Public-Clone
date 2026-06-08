using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Account
{
    public int Idaccount { get; set; }

    public string Aname { get; set; } = null!;

    public string? Adescription { get; set; }

    public string? Abankno { get; set; }

    public string? Anotes { get; set; }

    public int? Idtax { get; set; }

    public int? Idaccountt { get; set; }

    public virtual Accounttype? IdaccounttNavigation { get; set; }

    public virtual Tax? IdtaxNavigation { get; set; }

    public virtual ICollection<Itservice> ItserviceIdaccountnNavigations { get; set; } = new List<Itservice>();

    public virtual ICollection<Itservice> ItserviceIdaccountpNavigations { get; set; } = new List<Itservice>();

    public virtual ICollection<Itservice> ItserviceIdaccountsNavigations { get; set; } = new List<Itservice>();
}
