using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Jobtype
{
    public int Idjobtype { get; set; }

    public string Jtname { get; set; } = null!;

    public int? Idjobfk { get; set; }

    public virtual Jobtype? IdjobfkNavigation { get; set; }

    public virtual ICollection<Jobtype> InverseIdjobfkNavigation { get; set; } = new List<Jobtype>();

    public virtual ICollection<Jobinfo> Jobinfos { get; set; } = new List<Jobinfo>();
}
