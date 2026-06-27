using System;

namespace Domain.Entities.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string UserRole { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
    }
}