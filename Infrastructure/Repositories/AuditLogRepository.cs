using Domain.Common.Audit;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;

        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuditLog>> GetRecentAsync(int take = 200)
        {
            return await _context.AuditLogs
                .OrderByDescending(x => x.CreatedAt)
                .Take(take)
                .ToListAsync();
        }
    }
}