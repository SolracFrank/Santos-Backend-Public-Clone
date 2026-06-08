namespace Domain.Entities.Dtos;

public class EstimatedetailDto
{
    public int Idestimatedetail { get; set; }
    public int Idestimate { get; set; }
    public int? Iditem { get; set; }
    public string? Itemname { get; set; }
    public string? Description { get; set; }
    public decimal Qty { get; set; }
    public decimal Cost { get; set; }
    public decimal Total { get; set; }
    public EstimateDto IdestimateNavigation { get; set; } = null!;
    public ItserviceDto? IditemNavigation { get; set; }
}