using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Estimatedetail
{
    public int Idestimatedetail { get; set; }

    public int Idestimate { get; set; }

    public int? Iditem { get; set; }

    public string? Itemname { get; set; }

    public string? Description { get; set; }

    public decimal Qty { get; set; }

    public decimal Cost { get; set; }

    public decimal Total { get; set; }

    public virtual Estimate IdestimateNavigation { get; set; } = null!;

    public virtual Itservice? IditemNavigation { get; set; }
}
