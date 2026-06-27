namespace Domain.Entities.Dtos
{
    public class CreateEmployeeRequestDto
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}