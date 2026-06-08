using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Payt
{
    public int Idpayt { get; set; }

    public string? Ptname { get; set; }

    public virtual ICollection<Paymd> Paymds { get; set; } = new List<Paymd>();
}
