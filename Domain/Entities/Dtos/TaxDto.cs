namespace Domain.Entities.Dtos;

public class TaxDto
{
    public int Idtax { get; set; }
    public string Tname { get; set; } = null!;
    public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();
}