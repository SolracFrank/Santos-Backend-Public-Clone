namespace Domain.Entities.Dtos;

public class PaytDto
{
    public int Idpayt { get; set; }
    public string? Ptname { get; set; }
    public List<PaymdDto> Paymds { get; set; } = new List<PaymdDto>();
}