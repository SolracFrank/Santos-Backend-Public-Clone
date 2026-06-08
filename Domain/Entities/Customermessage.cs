using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Customermessage
{
    public int Idcustomermessage { get; set; }

    public string Message { get; set; } = null!;

    public virtual ICollection<Estimate> Estimates { get; set; } = new List<Estimate>();
}
