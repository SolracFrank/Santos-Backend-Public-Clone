using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Customer
{
    public int Idcustom { get; set; }

    public string Cname { get; set; } = null!;

    public int? Idcustomfk { get; set; }

    public virtual Customer? IdcustomfkNavigation { get; set; }

    public virtual ICollection<Customer> InverseIdcustomfkNavigation { get; set; } = new List<Customer>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
