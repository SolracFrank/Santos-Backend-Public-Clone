using Domain.Entities.Dtos;
using System.Threading.Tasks;

namespace Domain.Common.Audit
{
    public interface IAuditService
    {
        Task LogAsync(AuditLogDto auditLog);
    }
}