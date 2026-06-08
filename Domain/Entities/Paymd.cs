using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Paymd
{
    public int Idpaymd { get; set; }

    public string Pmdname { get; set; } = null!;

    public int? Idpayt { get; set; }

    public virtual Payt? IdpaytNavigation { get; set; }

    public virtual ICollection<Itservice> Itservices { get; set; } = new List<Itservice>();
}
