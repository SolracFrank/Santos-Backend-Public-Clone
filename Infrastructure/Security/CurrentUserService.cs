using Domain.Common.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public int IdLoginUser
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst("IdLoginUser")?
                    .Value;

                return int.TryParse(value, out var id)
                    ? id
                    : 0;
            }
        }

        public string? Username =>
            _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.Name)?
                .Value;

        public string? UserRole =>
            _httpContextAccessor.HttpContext?
                .User
                .FindFirst(ClaimTypes.Role)?
                .Value;
    }
}