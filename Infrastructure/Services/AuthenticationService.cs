using Domain.Common.Security;
using Domain.Entities.Dtos;
using Domain.Common.Audit;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILoginUserRepository _loginUserRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IAuditService _auditService;

        public AuthenticationService(
            ILoginUserRepository loginUserRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService,
            IAuditService auditService)
        {
            _loginUserRepository = loginUserRepository;

            _passwordHasher = passwordHasher;

            _jwtTokenService = jwtTokenService;

            _auditService = auditService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _loginUserRepository.GetByUsernameAsync(request.Username);

            if (user == null)
            {
                await _auditService.LogAsync(new AuditLogDto
                {
                    Username = request.Username,
                    Action = AuditActions.LoginFailed,
                    EntityName = "Authentication",
                    Description = "Invalid username or password.",
                    Succeeded = false,
                    Severity = AuditSeverity.Warning
                });

                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var passwordIsValid = _passwordHasher.VerifyPassword(
                request.Password,
                user.PasswordHash);
            if (!passwordIsValid)
            {
                await _auditService.LogAsync(new AuditLogDto
                {
                    IdLoginUser = user.Idloginuser,
                    Username = user.Username,
                    Action = AuditActions.LoginFailed,
                    EntityName = "Authentication",
                    Description = "Invalid username or password.",
                    PerformedByRole = user.UserRole,
                    Succeeded = false,
                    Severity = AuditSeverity.Warning
                });

                throw new UnauthorizedAccessException("Invalid username or password.");
            }
            if (!user.IsActive)
            {
                await _auditService.LogAsync(new AuditLogDto
                {
                    IdLoginUser = user.Idloginuser,
                    Username = user.Username,
                    Action = AuditActions.LoginFailed,
                    EntityName = "Authentication",
                    Description = "Login attempt with inactive account.",
                    PerformedByRole = user.UserRole,
                    Succeeded = false,
                    Severity = AuditSeverity.Warning
                });

                throw new UnauthorizedAccessException("Invalid username or password.");
            }
            var response = _jwtTokenService.GenerateToken(user);
            response.CanCreateUsers = user.CanCreateUsers;
            response.CanEditUsers = user.CanEditUsers;
            response.CanDisableUsers = user.CanDisableUsers;
            response.CanViewAuditLog = user.CanViewAuditLog;
            response.CanManagePermissions = user.CanManagePermissions;
            response.CanManageItems = user.CanManageItems;
            response.CanDeleteItems = user.CanDeleteItems;
            user.LastLogin = DateTime.UtcNow;

            await _loginUserRepository.UpdateAsync(user);
            await _auditService.LogAsync(new AuditLogDto
            {
                IdLoginUser = user.Idloginuser,
                Username = user.Username,
                Action = AuditActions.Login,
                EntityName = "Authentication",
                Description = "Successful login.",
                PerformedByRole = user.UserRole,
                Succeeded = true,
                Severity = AuditSeverity.Information
            });

            return response;
        }
    }
}