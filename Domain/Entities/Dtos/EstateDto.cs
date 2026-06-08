namespace Domain.Entities.Dtos;

public class EstateDto
{
    public int Idestate { get; set; }
    public string Esname { get; set; } = null!;
    public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
    public List<OthernameDto> Othernames { get; set; } = new List<OthernameDto>();
}