using Application.Features.LoginUsers.Commands;
using Application.Features.LoginUsers.Mappers;
using Domain.Common.Audit;
using Domain.Common.Security;
using Domain.Entities;
using Domain.Entities.Dtos;
using Domain.Exceptions;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.LoginUsers.Handlers;

public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, Result<LoginUserDto>>
{
    private readonly ILoginUserRepository _loginUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditService _auditService;

    public ResetPasswordCommandHandler(
        ILoginUserRepository loginUserRepository,
        IPasswordHasher passwordHasher,
        ILogger<ResetPasswordCommandHandler> logger,
        IPermissionService permissionService,
        ICurrentUserService currentUserService,
        IAuditService auditService)
    {
        _loginUserRepository = loginUserRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _permissionService = permissionService;
        _currentUserService = currentUserService;
        _auditService = auditService;
    }

    public async Task<Result<LoginUserDto>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting handler {HandlerName}.",
            nameof(ResetPasswordCommandHandler));

        try
        {
            var canEditUsers = await _permissionService.CanEditUsersAsync(
                _currentUserService.IdLoginUser);

            if (!canEditUsers)
            {
                throw new ForbiddenException(
                    "You do not have permission to reset passwords.",
                    "USER_RESET_PASSWORD_FORBIDDEN");
            }
            var user = await _loginUserRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                throw new NotFoundException(
                    $"Login user with id {request.Id} was not found.",
                    "LOGINUSER_NOT_FOUND");
            }

            if (user.Idloginuser != _currentUserService.IdLoginUser &&
                _currentUserService.UserRole != LoginUser.AdministratorRole &&
                (user.UserRole == LoginUser.AdministratorRole || user.CanManagePermissions ||
                    user.CanCreateUsers || user.CanEditUsers || user.CanDisableUsers ||
                    user.CanViewAuditLog || user.CanManageItems || user.CanDeleteItems || user.CanSendEmails))
            {
                throw new ForbiddenException("Only administrators can reset another privileged account.", "USER_RESET_PASSWORD_FORBIDDEN");
            }

            user.PasswordHash =
                _passwordHasher.HashPassword(request.NewPassword);

            user.SecurityStamp = Guid.NewGuid();

            await _loginUserRepository.UpdateAsync(user);
            await _auditService.LogAsync(new AuditLogDto
            {
                IdLoginUser = _currentUserService.IdLoginUser,
                Username = _currentUserService.Username,
                Action = AuditActions.UserPasswordChanged,
                EntityName = "LoginUsers",
                EntityId = user.Idloginuser.ToString(),
                Description = $"Changed password for '{user.Username}'.",
                PerformedByRole = _currentUserService.UserRole,
                Succeeded = true,
                Severity = AuditSeverity.Information
            });
            _logger.LogInformation(
                "Password reset successfully for {Username}.",
                user.Username);

            return new Result<LoginUserDto>(
                LoginUserMapper.ToDto(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while handling {HandlerName}.",
                nameof(ResetPasswordCommandHandler));

            return new Result<LoginUserDto>(ex);
        }
    }
}
