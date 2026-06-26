using System;

namespace Domain.Entities.Dtos
{
    public class LoginUserDto
    {
        public int Idloginuser { get; set; }

        public string Username { get; set; } = null!;

        public string UserRole { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLogin { get; set; }
    }
}