namespace Domain.Entities.Dtos;

public class EmployeeDto
{
    public int Idemployee { get; set; }
    public string Ename { get; set; } = null!;
    public string Elastname { get; set; } = null!;
    public string? Eaddress { get; set; }
    public string? Ecity { get; set; }
    public int? Ezip { get; set; }
    public string? Eprint { get; set; }
    public string? Ephone { get; set; }
    public string? Eemail { get; set; }
    public int? Idstate { get; set; }
    public EstateDto? IdstateNavigation { get; set; }
    public List<RepsaleDto> Repsales { get; set; } = new List<RepsaleDto>();
}