namespace Domain.Entities.Dtos;

public class CustomerTransaction
{
    public int? Idestimate { get; set; }
    public int? Idinvoice { get; set; }
    public int? Idpayment { get; set; }

    public string Type { get; set; } = "";
    public string Num { get; set; } = "";
    public DateTime Date { get; set; }
    public string Account { get; set; } = "";
    public decimal Amount { get; set; }
    public string? Status { get; set; }

    
    public string? PaymentType { get; set; }
}