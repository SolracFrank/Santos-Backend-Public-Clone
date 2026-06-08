namespace Domain.Entities.Dtos;

public class ItserviceDto
{
    public int Iditem { get; set; }
    public string Iname { get; set; } = null!;
    public string? Idescription { get; set; }
    public string? Irate { get; set; }
    public string? Idescpurchase { get; set; }
    public string? Icost { get; set; }
    public string? Idescsales { get; set; }
    public string? Isales { get; set; }
    public int? Iditemt { get; set; }
    public int? Idaccountp { get; set; }
    public int? Idaccounts { get; set; }
    public int? Idaccountn { get; set; }
    public int? Idpaymd { get; set; }
    public int? Iditemfk { get; set; }
    public List<EstimatedetailDto> Estimatedetails { get; set; } = new List<EstimatedetailDto>();
    public AccountDto? IdaccountnNavigation { get; set; }
    public AccountDto? IdaccountpNavigation { get; set; }
    public AccountDto? IdaccountsNavigation { get; set; }
    public ItserviceDto? IditemfkNavigation { get; set; }
    public ItemtypeDto? IditemtNavigation { get; set; }
    public PaymdDto? IdpaymdNavigation { get; set; }
    public List<ItserviceDto> InverseIditemfkNavigation { get; set; } = new List<ItserviceDto>();
}