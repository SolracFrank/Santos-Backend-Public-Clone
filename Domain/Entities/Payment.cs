using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Payment
{
    public int Idpayment { get; set; }

    public int Paymentnumber { get; set; }

    public DateTime Paymentdate { get; set; }

    public decimal Amount { get; set; }

    public string? Memo { get; set; }

    public string? Reference { get; set; }

    public int? Idusers { get; set; }

    public int? Idpaymd { get; set; }

    public int? Idaccount { get; set; }

    public virtual ICollection<PaymentApply> PaymentApplies { get; set; } = new List<PaymentApply>();
}
