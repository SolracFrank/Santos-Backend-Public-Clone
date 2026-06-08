namespace Domain.Entities.Dtos;

public class RepsaleDto
{
    public int Idrepsales { get; set; }
    public string Rinitials { get; set; } = null!;
    public int? Idemployee { get; set; }
    public int? Idothern { get; set; }
    public List<EstimateDto> Estimates { get; set; } = new List<EstimateDto>();
    public EmployeeDto? IdemployeeNavigation { get; set; }
    public OthernameDto? IdothernNavigation { get; set; }
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}