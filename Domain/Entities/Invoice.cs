using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Invoice
{
    public int Idinvoice { get; set; }

    public int Invoicenumber { get; set; }

    public DateTime Invoicedate { get; set; }

    public DateTime? Duedate { get; set; }

    public DateTime? Dteordered { get; set; }

    public string? Ponumber { get; set; }

    public string? Memo { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Markup { get; set; }

    public decimal Total { get; set; }

    public bool? Activeinvoice { get; set; }

    public int? Idusers { get; set; }

    public int? Idterms { get; set; }

    public int? Idrepsales { get; set; }

    public int? Idcustomermessage { get; set; }

    public string? Customermessagetext { get; set; }

    public int? Createdfromestimateid { get; set; }

    public DateTime? Dateordered { get; set; }

    public decimal Openbalance { get; set; }

    public int? Idtaxrate { get; set; }

    public decimal Taxrate { get; set; }

    public decimal Taxamount { get; set; }

    public DateTime LastModifiedUtc { get; set; }

    public virtual TaxRate? IdtaxrateNavigation { get; set; }

    public virtual ICollection<Invoicedetail> Invoicedetails { get; set; } = new List<Invoicedetail>();

    public virtual ICollection<PaymentApply> PaymentApplies { get; set; } = new List<PaymentApply>();
}
