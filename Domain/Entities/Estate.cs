using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Estate
{
    public int Idestate { get; set; }

    public string Esname { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Othername> Othernames { get; set; } = new List<Othername>();
}
