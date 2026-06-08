using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class PaymentApply
{
    public int Idpaymentapply { get; set; }

    public int Idpayment { get; set; }

    public int Idinvoice { get; set; }

    public decimal AmountApplied { get; set; }

    public virtual Invoice IdinvoiceNavigation { get; set; } = null!;

    public virtual Payment IdpaymentNavigation { get; set; } = null!;
}
