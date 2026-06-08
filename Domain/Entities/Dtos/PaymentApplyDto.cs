namespace Domain.Entities.Dtos;

public class PaymentApplyDto
{
    public int Idpaymentapply { get; set; }
    public int Idpayment { get; set; }
    public int Idinvoice { get; set; }
    public decimal AmountApplied { get; set; }
    public InvoiceDto IdinvoiceNavigation { get; set; } = null!;
    public PaymentDto IdpaymentNavigation { get; set; } = null!;
}