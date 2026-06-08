namespace Domain.Entities.Dtos;

public class AccounttypeDto
{
    public int Idaccountt { get; set; }
    public string Atype { get; set; } = null!;
    public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();
}