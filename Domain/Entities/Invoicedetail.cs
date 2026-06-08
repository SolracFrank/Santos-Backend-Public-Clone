using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Invoicedetail
{
    public int Idinvoicedetail { get; set; }

    public int Idinvoice { get; set; }

    public int? Iditem { get; set; }

    public string? Itemname { get; set; }

    public string? Description { get; set; }

    public decimal Qty { get; set; }

    public decimal Cost { get; set; }

    public decimal Total { get; set; }

    public virtual Invoice IdinvoiceNavigation { get; set; } = null!;
}
