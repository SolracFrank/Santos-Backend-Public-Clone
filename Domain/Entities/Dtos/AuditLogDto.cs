using Domain.Common.Audit;
using System;

namespace Domain.Entities.Dtos
{
    public class AuditLogDto
    {
        public int Idauditlog { get; set; }

        public int? IdLoginUser { get; set; }

        public string? Username { get; set; }

        public string Action { get; set; } = null!;

        public string? EntityName { get; set; }

        public string? EntityId { get; set; }

        public string? Description { get; set; }

        public string? IpAddress { get; set; }

        public string? PerformedByRole { get; set; }

        public bool Succeeded { get; set; }

        public AuditSeverity Severity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}