namespace Domain.Entities.Dtos;

public class UserDto
{
    public int Idusers { get; set; }
    public string Ucustomer { get; set; } = null!;
    public string? Ubalance { get; set; }
    public DateOnly? Udate { get; set; }
    public int? Idaddress { get; set; }
    public int? Idterms { get; set; }
    public int? Idcustomer { get; set; }
    public int? Idrepsales { get; set; }
    public int? Idjobinfo { get; set; }
    public string? Unotes { get; set; }
    public List<EstimateDto> Estimates { get; set; } = new List<EstimateDto>();
    public AddressDto? IdaddressNavigation { get; set; }
    public CustomerDto? IdcustomerNavigation { get; set; }
    public JobinfoDto? IdjobinfoNavigation { get; set; }
    public RepsaleDto? IdrepsalesNavigation { get; set; }
    public TermDto? IdtermsNavigation { get; set; }
}