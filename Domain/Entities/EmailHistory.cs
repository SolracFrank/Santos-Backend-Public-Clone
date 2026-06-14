namespace Domain.Entities;

public partial class EmailHistory
{
    public int Idemailhistory { get; set; }
    public int Idusers { get; set; }
    public string Entitytype { get; set; } = null!;
    public string Documentnumber { get; set; } = null!;
    public string Emailto { get; set; } = null!;
    public string? Subject { get; set; }
    public DateTime Datesent { get; set; }

    public virtual User IdusersNavigation { get; set; } = null!;
}