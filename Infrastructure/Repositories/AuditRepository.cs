using Domain.Common.Audit;
using Domain.Entities;
using Infrastructure.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _context;

        public AuditRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);

            await _context.SaveChangesAsync();
        }
    }
}