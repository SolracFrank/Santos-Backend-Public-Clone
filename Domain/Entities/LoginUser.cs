using System;

namespace Domain.Entities
{
    public class LoginUser
    {
        public int Idloginuser { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string UserRole { get; set; }

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

        public Guid SecurityStamp { get; set; }

        public string? EmailFromAddress { get; set; }

        public string? EmailFromName { get; set; }

        public bool CanSendEmails { get; set; } = true;

        public void GrantAdministratorPermissions()
        {
            CanCreateUsers = true;
            CanEditUsers = true;
            CanDisableUsers = true;
            CanViewAuditLog = true;
            CanManagePermissions = true;

            CanManageItems = true;
            CanDeleteItems = true;
        }
    }
}