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

public class UpdateLoginUserCommandHandler
    : IRequestHandler<UpdateLoginUserCommand, Result<LoginUserDto>>
{
    private readonly ILoginUserRepository _loginUserRepository;
    private readonly ILogger<UpdateLoginUserCommandHandler> _logger;
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditService _auditService;

    public UpdateLoginUserCommandHandler(
        ILoginUserRepository loginUserRepository,
        ILogger<UpdateLoginUserCommandHandler> logger,
        IPermissionService permissionService,
        ICurrentUserService currentUserService,
        IAuditService auditService)
    {
        _loginUserRepository = loginUserRepository;
        _logger = logger;
        _permissionService = permissionService;
        _currentUserService = currentUserService;
        _auditService = auditService;
    }

    public async Task<Result<LoginUserDto>> Handle(
        UpdateLoginUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting handler {HandlerName}.",
            nameof(UpdateLoginUserCommandHandler));

        try
        {
            var canEditUsers = await _permissionService.CanEditUsersAsync(
            _currentUserService.IdLoginUser);

            var canManagePermissions = await _permissionService.CanManagePermissionsAsync(
                _currentUserService.IdLoginUser);

            if (!canEditUsers || !canManagePermissions)
            {
                throw new ForbiddenException(
                    "You do not have permission to update users or permissions.",
                    "USER_UPDATE_FORBIDDEN");
            }
            if (request.Id != request.Idloginuser)
                throw new BadRequestException("Id mismatch.");

            var user = await _loginUserRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                throw new NotFoundException(
                    $"Login user with id {request.Id} was not found.",
                    "LOGINUSER_NOT_FOUND");
            }

            if ((user.UserRole == LoginUser.AdministratorRole || request.UserRole == LoginUser.AdministratorRole) &&
                _currentUserService.UserRole != LoginUser.AdministratorRole)
            {
                throw new ForbiddenException("Only administrators can manage administrator accounts.", "USER_PERMISSIONS_FORBIDDEN");
            }

            var existingUser = await _loginUserRepository
                .GetByUsernameAsync(request.Username);

            if (existingUser is not null &&
                existingUser.Idloginuser != user.Idloginuser)
            {
                throw new ConflictException(
                    "Username already exists.",
                    "LOGINUSER_ALREADY_EXISTS");
            }

            user.Username = request.Username;
            user.UserRole = request.UserRole;
            user.IsActive = request.IsActive;
            user.SecurityStamp = Guid.NewGuid();

            user.CanCreateUsers = request.CanCreateUsers;
            user.CanEditUsers = request.CanEditUsers;
            user.CanDisableUsers = request.CanDisableUsers;
            user.CanViewAuditLog = request.CanViewAuditLog;
            user.CanManagePermissions = request.CanManagePermissions;
            user.CanManageItems = request.CanManageItems;
            user.CanDeleteItems = request.CanDeleteItems;

            user.EmailFromAddress = request.EmailFromAddress;
            user.EmailFromName = request.EmailFromName;
            user.CanSendEmails = request.CanSendEmails;

            if (user.UserRole == LoginUser.AdministratorRole)
            {
                user.GrantAdministratorPermissions();
            }

            await _loginUserRepository.UpdateAsync(user);
            await _auditService.LogAsync(new AuditLogDto
            {
                IdLoginUser = _currentUserService.IdLoginUser,
                Username = _currentUserService.Username,
                Action = AuditActions.UserUpdated,
                EntityName = "LoginUsers",
                EntityId = user.Idloginuser.ToString(),
                Description = $"Updated user '{user.Username}'.",
                PerformedByRole = _currentUserService.UserRole,
                Succeeded = true,
                Severity = AuditSeverity.Information
            });

            _logger.LogInformation(
                "Login user {Username} updated successfully.",
                user.Username);

            return new Result<LoginUserDto>(
                LoginUserMapper.ToDto(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while handling {HandlerName}.",
                nameof(UpdateLoginUserCommandHandler));

            return new Result<LoginUserDto>(ex);
        }
    }
}
