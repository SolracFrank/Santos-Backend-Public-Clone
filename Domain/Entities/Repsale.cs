using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Repsale
{
    public int Idrepsales { get; set; }

    public string Rinitials { get; set; } = null!;

    public int? Idemployee { get; set; }

    public int? Idothern { get; set; }

    public virtual ICollection<Estimate> Estimates { get; set; } = new List<Estimate>();

    public virtual Employee? IdemployeeNavigation { get; set; }

    public virtual Othername? IdothernNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
