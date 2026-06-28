using System;

namespace Domain.Entities.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string UserRole { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public bool CanCreateUsers { get; set; }

        public bool CanEditUsers { get; set; }

        public bool CanDisableUsers { get; set; }

        public bool CanViewAuditLog { get; set; }

        public bool CanManagePermissions { get; set; }

        public bool CanManageItems { get; set; }

        public bool CanDeleteItems { get; set; }
    }
}