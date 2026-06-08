namespace Domain.Entities.Dtos;

public class PaymdDto
{
    public int Idpaymd { get; set; }
    public string Pmdname { get; set; } = null!;
    public int? Idpayt { get; set; }
    public PaytDto? IdpaytNavigation { get; set; }
    public List<ItserviceDto> Itservices { get; set; } = new List<ItserviceDto>();
}