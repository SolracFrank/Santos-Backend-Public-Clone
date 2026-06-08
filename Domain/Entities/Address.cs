using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Address
{
    public int Idaddress { get; set; }

    public string? Acompany { get; set; }

    public string? Afirstname { get; set; }

    public string? Alastname { get; set; }

    public string? Ajobtitle { get; set; }

    public string? Amainphone { get; set; }

    public string? Aemail { get; set; }

    public string? Aother { get; set; }

    public string? Accemail { get; set; }

    public string? Ainvoice { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
