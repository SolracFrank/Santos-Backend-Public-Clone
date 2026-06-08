namespace Domain.Entities.Dtos;

public class OthernameDto
{
    public int Idother { get; set; }
    public string Oname { get; set; } = null!;
    public string Olastname { get; set; } = null!;
    public string? Oaddress { get; set; }
    public string? Ocity { get; set; }
    public string? Ophone { get; set; }
    public string? Oemail { get; set; }
    public int? Ozip { get; set; }
    public int? Idstate { get; set; }
    public EstateDto? IdstateNavigation { get; set; }
    public List<RepsaleDto> Repsales { get; set; } = new List<RepsaleDto>();
}