using Domain.Common.Security;
using Domain.Entities;
using Domain.Entities.Dtos;
using Domain.Models.ConfigurationModels;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtTokenService(IOptions<JwtConfiguration> jwtOptions)
        {
            _jwtConfiguration = jwtOptions.Value;
        }

        public LoginResponseDto GenerateToken(LoginUser user)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationMinutes);
            var claims = new List<Claim>
            {
            new Claim("IdLoginUser", user.Idloginuser.ToString()),

            new Claim(ClaimTypes.Name, user.Username),

            new Claim(ClaimTypes.Role, user.UserRole),

            new Claim("SecurityStamp", user.SecurityStamp.ToString())
            };
            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return new LoginResponseDto
            {
                Token = tokenString,
                Username = user.Username,
                UserRole = user.UserRole,
                ExpiresAt = expiresAt
            };
        }
    }
}