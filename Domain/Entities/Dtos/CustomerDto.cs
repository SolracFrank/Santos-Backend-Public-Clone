namespace Domain.Entities.Dtos;

public class CustomerDto
{
    public int Idcustom { get; set; }
    public string Cname { get; set; } = null!;
    public int? Idcustomfk { get; set; }
    public CustomerDto? IdcustomfkNavigation { get; set; }
    public List<CustomerDto> InverseIdcustomfkNavigation { get; set; } = new List<CustomerDto>();
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}