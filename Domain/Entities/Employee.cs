using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Employee
{
    public int Idemployee { get; set; }

    public string Ename { get; set; } = null!;

    public string Elastname { get; set; } = null!;

    public string? Eaddress { get; set; }

    public string? Ecity { get; set; }

    public int? Ezip { get; set; }

    public string? Eprint { get; set; }

    public string? Ephone { get; set; }

    public string? Eemail { get; set; }

    public int? Idstate { get; set; }

    public virtual Estate? IdstateNavigation { get; set; }

    public virtual ICollection<Repsale> Repsales { get; set; } = new List<Repsale>();
}
