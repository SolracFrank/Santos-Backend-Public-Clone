using Domain.Entities;

namespace Domain.Common.Security
{
    public interface IPermissionService
    {
        Task<bool> CanCreateUsersAsync(int idLoginUser);

        Task<bool> CanEditUsersAsync(int idLoginUser);

        Task<bool> CanDisableUsersAsync(int idLoginUser);

        Task<bool> CanViewAuditLogAsync(int idLoginUser);

        Task<bool> CanManagePermissionsAsync(int idLoginUser);

        Task<bool> CanManageItemsAsync(int idLoginUser);

        Task<bool> CanDeleteItemsAsync(int idLoginUser);
    }
}