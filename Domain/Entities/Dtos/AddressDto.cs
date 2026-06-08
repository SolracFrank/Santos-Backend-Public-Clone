namespace Domain.Entities.Dtos;

public class AddressDto
{
    public int Idaddress { get; set; }
    public string? Acompany { get; set; }
    public string? Afirstname { get; set; }
    public string? Alastname { get; set; }
    public string? Ajobtitle { get; set; }
    public string? Amainphone { get; set; }
    public string? Aemail { get; set; }
    public string? Aother { get; set; }
    public string? Accemail { get; set; }
    public string? Ainvoice { get; set; }
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}