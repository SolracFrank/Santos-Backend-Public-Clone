using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Common.Audit
{
    public interface IAuditRepository
    {
        Task AddAsync(AuditLog auditLog);
    }
}