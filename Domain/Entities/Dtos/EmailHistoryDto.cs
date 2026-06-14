namespace Domain.Entities.Dtos;

public class EmailHistoryDto
{
    public int Idemailhistory { get; set; }
    public int Idusers { get; set; }
    public string Entitytype { get; set; } = "";
    public string Documentnumber { get; set; } = "";
    public string Emailto { get; set; } = "";
    public string? Subject { get; set; }
    public DateTime Datesent { get; set; }
}