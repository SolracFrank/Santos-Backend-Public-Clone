using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Othername
{
    public int Idother { get; set; }

    public string Oname { get; set; } = null!;

    public string Olastname { get; set; } = null!;

    public string? Oaddress { get; set; }

    public string? Ocity { get; set; }

    public string? Ophone { get; set; }

    public string? Oemail { get; set; }

    public int? Ozip { get; set; }

    public int? Idstate { get; set; }

    public virtual Estate? IdstateNavigation { get; set; }

    public virtual ICollection<Repsale> Repsales { get; set; } = new List<Repsale>();
}
