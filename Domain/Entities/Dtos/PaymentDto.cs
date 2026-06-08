namespace Domain.Entities.Dtos;

public class PaymentDto
{
    public int Idpayment { get; set; }
    public int Paymentnumber { get; set; }
    public DateTime Paymentdate { get; set; }
    public decimal Amount { get; set; }
    public string? Memo { get; set; }
    public int? Idusers { get; set; }
    public int? Idpaymd { get; set; }
    public int? Idaccount { get; set; }
    public List<PaymentApplyDto> PaymentApplies { get; set; } = new List<PaymentApplyDto>();
}