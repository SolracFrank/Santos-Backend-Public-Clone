namespace Domain.Entities.Dtos;

public class JobtypeDto
{
    public int Idjobtype { get; set; }
    public string Jtname { get; set; } = null!;
    public int? Idjobfk { get; set; }
    public JobtypeDto? IdjobfkNavigation { get; set; }
    public List<JobtypeDto> InverseIdjobfkNavigation { get; set; } = new List<JobtypeDto>();
    public List<JobinfoDto> Jobinfos { get; set; } = new List<JobinfoDto>();
}