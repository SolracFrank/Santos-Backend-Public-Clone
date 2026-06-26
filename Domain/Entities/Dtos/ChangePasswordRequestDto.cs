namespace Domain.Entities.Dtos
{
    public class ChangePasswordRequestDto
    {
        public int Idloginuser { get; set; }

        public string CurrentPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;
    }
}