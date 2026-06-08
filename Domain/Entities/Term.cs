using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Term
{
    public int Idterms { get; set; }

    public string Tname { get; set; } = null!;

    public int? Tnets { get; set; }

    public decimal? Tperccs { get; set; }

    public int? Tnetd { get; set; }

    public int? Tdags { get; set; }

    public decimal? Tpercd { get; set; }

    public virtual ICollection<Estimate> Estimates { get; set; } = new List<Estimate>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
