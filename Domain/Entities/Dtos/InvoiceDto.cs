namespace Domain.Entities.Dtos;

public class InvoiceDto
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
    public decimal Ppenbalance { get; set; }
    public bool? Activeinvoice { get; set; }
    public int? Idusers { get; set; }
    public int? Idterms { get; set; }
    public int? Idrepsales { get; set; }
    public int? Idcustomermessage { get; set; }
    public string? Customermessagetext { get; set; }
    public int? Createdfromestimateid { get; set; }
    public DateTime? Dateordered { get; set; }
    public decimal Openbalance { get; set; }
    public List<InvoicedetailDto> Invoicedetails { get; set; } = new List<InvoicedetailDto>();
    public List<PaymentApplyDto> PaymentApplies { get; set; } = new List<PaymentApplyDto>();
}