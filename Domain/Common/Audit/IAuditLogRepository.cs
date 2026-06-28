using Domain.Entities;

namespace Domain.Common.Audit
{
    public interface IAuditLogRepository
    {
        Task<List<AuditLog>> GetRecentAsync(int take = 200);
    }
}