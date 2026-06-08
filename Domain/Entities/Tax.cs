using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Tax
{
    public int Idtax { get; set; }

    public string Tname { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
