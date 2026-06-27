using Domain.Common.Audit;
using Domain.Entities;
using Domain.Entities.Dtos;

namespace Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;

        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task LogAsync(AuditLogDto auditLog)
        {
            var entity = new AuditLog
            {
                Idloginuser = auditLog.IdLoginUser,
                Username = auditLog.Username,
                Action = auditLog.Action,
                EntityName = auditLog.EntityName,
                EntityId = auditLog.EntityId,
                Description = auditLog.Description,
                IpAddress = auditLog.IpAddress,
                PerformedByRole = auditLog.PerformedByRole,
                Succeeded = auditLog.Succeeded,
                Severity = auditLog.Severity,
                CreatedAt = DateTime.UtcNow
            };

            await _auditRepository.AddAsync(entity);
        }
    }
}