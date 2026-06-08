namespace Domain.Entities.Dtos;

public class EstimateDto
{
    public int Idestimate { get; set; }
    public int Estimatenumber { get; set; }
    public DateTime Estimatedate { get; set; }
    public DateTime? Expdate { get; set; }
    public DateTime? Datetobecompleted { get; set; }
    public DateTime? Dateordered { get; set; }
    public string? Memo { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Markup { get; set; }
    public decimal Total { get; set; }
    public bool? Activeestimate { get; set; }
    public decimal Openbalance { get; set; }
    public int? Idusers { get; set; }
    public int? Idterms { get; set; }
    public int? Idrepsales { get; set; }
    public int? Idcustomermessage { get; set; }
    public string? Customermessagetext { get; set; }
    public bool IsCopy { get; set; }
    public int? CopiedFromEstimateId { get; set; }
    public EstimateDto? CopiedFromEstimate { get; set; }
    public List<EstimatedetailDto> Estimatedetails { get; set; } = new List<EstimatedetailDto>();
    public CustomermessageDto? IdcustomermessageNavigation { get; set; }
    public RepsaleDto? IdrepsalesNavigation { get; set; }
    public TermDto? IdtermsNavigation { get; set; }
    public UserDto? IdusersNavigation { get; set; }
    public List<EstimateDto> InverseCopiedFromEstimate { get; set; } = new List<EstimateDto>();
}