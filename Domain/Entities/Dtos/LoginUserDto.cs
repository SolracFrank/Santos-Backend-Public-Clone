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

        public bool CanCreateUsers { get; set; }

        public bool CanEditUsers { get; set; }

        public bool CanDisableUsers { get; set; }

        public bool CanViewAuditLog { get; set; }

        public bool CanManagePermissions { get; set; }

        public bool CanManageItems { get; set; }

        public bool CanDeleteItems { get; set; }
    }
}