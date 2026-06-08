using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Jobinfo
{
    public int Idjobinfo { get; set; }

    public string? Jdescription { get; set; }

    public string Jjobstatus { get; set; } = null!;

    public int? Idjobtype { get; set; }

    public virtual Jobtype? IdjobtypeNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
