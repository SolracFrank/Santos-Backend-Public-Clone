using Domain.Entities;
using Domain.Entities.Dtos;

namespace Application.Features.LoginUsers.Mappers
{
    public static class LoginUserMapper
    {
        public static LoginUserDto ToDto(LoginUser entity)
        {
            return new LoginUserDto
            {
                Idloginuser = entity.Idloginuser,
                Username = entity.Username,
                UserRole = entity.UserRole,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                LastLogin = entity.LastLogin,

                CanCreateUsers = entity.CanCreateUsers,
                CanEditUsers = entity.CanEditUsers,
                CanDisableUsers = entity.CanDisableUsers,
                CanViewAuditLog = entity.CanViewAuditLog,
                CanManagePermissions = entity.CanManagePermissions,
                CanManageItems = entity.CanManageItems,
                CanDeleteItems = entity.CanDeleteItems
            };
        }
    }
}