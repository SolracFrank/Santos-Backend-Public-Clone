using Domain.Entities;
using Domain.Entities.Dtos;

namespace Application.Features.AuditLogs.Mappers
{
    public static class AuditLogMapper
    {
        public static AuditLogDto ToDto(AuditLog entity)
        {
            return new AuditLogDto
            {
                Idauditlog = entity.Idauditlog,
                IdLoginUser = entity.Idloginuser,
                Username = entity.Username,
                Action = entity.Action,
                EntityName = entity.EntityName,
                EntityId = entity.EntityId,
                Description = entity.Description,
                IpAddress = entity.IpAddress,
                PerformedByRole = entity.PerformedByRole,
                Succeeded = entity.Succeeded,
                Severity = entity.Severity,
                CreatedAt = entity.CreatedAt
            };
        }
    }
}