using Domain.Entities;
using Domain.Entities.Dtos;

namespace Domain.Common.Security
{
    public interface IJwtTokenService
    {
        LoginResponseDto GenerateToken(LoginUser user);
    }
}