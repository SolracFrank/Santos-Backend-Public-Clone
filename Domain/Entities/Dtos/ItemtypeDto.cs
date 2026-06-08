namespace Domain.Entities.Dtos;

public class ItemtypeDto
{
    public int Iditemt { get; set; }
    public string Itname { get; set; } = null!;
    public List<ItserviceDto> Itservices { get; set; } = new List<ItserviceDto>();
}