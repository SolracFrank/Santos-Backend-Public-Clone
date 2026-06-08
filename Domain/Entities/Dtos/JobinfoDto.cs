namespace Domain.Entities.Dtos;

public class JobinfoDto
{
    public int Idjobinfo { get; set; }
    public string? Jdescription { get; set; }
    public string Jjobstatus { get; set; } = null!;
    public int? Idjobtype { get; set; }
    public JobtypeDto? IdjobtypeNavigation { get; set; }
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}