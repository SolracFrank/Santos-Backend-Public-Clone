namespace Domain.Entities.Dtos;

public class CustomermessageDto
{
    public int Idcustomermessage { get; set; }
    public string Message { get; set; } = null!;
    public List<EstimateDto> Estimates { get; set; } = new List<EstimateDto>();
}