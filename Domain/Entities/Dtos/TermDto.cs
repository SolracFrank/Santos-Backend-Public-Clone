namespace Domain.Entities.Dtos;

public class TermDto
{
    public int Idterms { get; set; }
    public string Tname { get; set; } = null!;
    public int? Tnets { get; set; }
    public decimal? Tperccs { get; set; }
    public int? Tnetd { get; set; }
    public int? Tdags { get; set; }
    public decimal? Tpercd { get; set; }
    public List<EstimateDto> Estimates { get; set; } = new List<EstimateDto>();
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}