using Domain.Common.Security;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;

        public PermissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CanCreateUsersAsync(int idLoginUser)
        {
            var user = await GetActiveUserAsync(idLoginUser);

            return user?.CanCreateUsers ?? false;
        }

        public async Task<bool> CanEditUsersAsync(int idLoginUser)
        {
            var user = await GetActiveUserAsync(idLoginUser);

            return user?.CanEditUsers ?? false;
        }

        public async Task<bool> CanDisableUsersAsync(int idLoginUser)
        {
            var user = await GetActiveUserAsync(idLoginUser);

            return user?.CanDisableUsers ?? false;
        }

        public async Task<bool> CanViewAuditLogAsync(int idLoginUser)
        {
            var user = await GetActiveUserAsync(idLoginUser);

            return user?.CanViewAuditLog ?? false;
        }

        public async Task<bool> CanManagePermissionsAsync(int idLoginUser)
        {
            var user = await GetActiveUserAsync(idLoginUser);

            return user?.CanManagePermissions ?? false;
        }

        public async Task<bool> CanManageItemsAsync(int idLoginUser)
        {
            var user = await GetActiveUserAsync(idLoginUser);

            return user?.CanManageItems ?? false;
        }

        public async Task<bool> CanDeleteItemsAsync(int idLoginUser)
        {
            var user = await GetActiveUserAsync(idLoginUser);

            return user?.CanDeleteItems ?? false;
        }

        private async Task<LoginUser?> GetActiveUserAsync(int idLoginUser)
        {
            return await _context.LoginUsers
                .FirstOrDefaultAsync(x =>
                    x.Idloginuser == idLoginUser &&
                    x.IsActive);
        }
    }
}