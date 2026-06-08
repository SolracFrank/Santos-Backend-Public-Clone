using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Accounttype
{
    public int Idaccountt { get; set; }

    public string Atype { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
