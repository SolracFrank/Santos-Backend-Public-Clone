using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Itemtype
{
    public int Iditemt { get; set; }

    public string Itname { get; set; } = null!;

    public virtual ICollection<Itservice> Itservices { get; set; } = new List<Itservice>();
}
