namespace Domain.Entities.Dtos;

public class AccountDto
{
    public int Idaccount { get; set; }
    public string Aname { get; set; } = null!;
    public string? Adescription { get; set; }
    public string? Abankno { get; set; }
    public string? Anotes { get; set; }
    public int? Idtax { get; set; }
    public int? Idaccountt { get; set; }
    public AccounttypeDto? IdaccounttNavigation { get; set; }
    public TaxDto? IdtaxNavigation { get; set; }
    public List<ItserviceDto> ItserviceIdaccountnNavigations { get; set; } = new List<ItserviceDto>();
    public List<ItserviceDto> ItserviceIdaccountpNavigations { get; set; } = new List<ItserviceDto>();
    public List<ItserviceDto> ItserviceIdaccountsNavigations { get; set; } = new List<ItserviceDto>();
}