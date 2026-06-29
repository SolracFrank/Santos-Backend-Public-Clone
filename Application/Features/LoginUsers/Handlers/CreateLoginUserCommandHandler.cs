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

public class CreateLoginUserCommandHandler
    : IRequestHandler<CreateLoginUserCommand, Result<LoginUserDto>>
{
    private readonly ILoginUserRepository _loginUserRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<CreateLoginUserCommandHandler> _logger;
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditService _auditService;

    public CreateLoginUserCommandHandler(
        ILoginUserRepository loginUserRepository,
        IPasswordHasher passwordHasher,
        ILogger<CreateLoginUserCommandHandler> logger,
        IPermissionService permissionService,
        ICurrentUserService currentUserService,
        IAuditService auditService
        )
    {
        _loginUserRepository = loginUserRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _permissionService = permissionService;
        _currentUserService = currentUserService;
        _auditService = auditService;

    }

    public async Task<Result<LoginUserDto>> Handle(
        CreateLoginUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting handler {HandlerName}.",
            nameof(CreateLoginUserCommandHandler));

        try
        {
            var canCreateUsers = await _permissionService.CanCreateUsersAsync(
                _currentUserService.IdLoginUser);

            if (!canCreateUsers)
            {
                throw new ForbiddenException(
                    "You do not have permission to create users.",
                    "USER_CREATE_FORBIDDEN");
            }

            var existingUser = await _loginUserRepository
                .GetByUsernameAsync(request.Username);

            if (existingUser is not null)
            {
                throw new ConflictException(
                    "Username already exists.",
                    "LOGINUSER_ALREADY_EXISTS");
            }

            var user = new LoginUser
            {
                Username = request.Username,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                UserRole = request.UserRole,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid(),

                CanCreateUsers = request.CanCreateUsers,
                CanEditUsers = request.CanEditUsers,
                CanDisableUsers = request.CanDisableUsers,
                CanViewAuditLog = request.CanViewAuditLog,
                CanManagePermissions = request.CanManagePermissions,
                CanManageItems = request.CanManageItems,
                CanDeleteItems = request.CanDeleteItems,

                EmailFromAddress = request.EmailFromAddress,
                EmailFromName = request.EmailFromName,
                CanSendEmails = request.CanSendEmails
            };

            if (user.UserRole == "Administrator")
            {
                user.GrantAdministratorPermissions();
            }

            await _loginUserRepository.AddAsync(user);
            await _auditService.LogAsync(new AuditLogDto
            {
                IdLoginUser = _currentUserService.IdLoginUser,
                Username = _currentUserService.Username,
                Action = AuditActions.UserCreated,
                EntityName = "LoginUsers",
                EntityId = user.Idloginuser.ToString(),
                Description = $"Created user '{user.Username}'.",
                PerformedByRole = _currentUserService.UserRole,
                Succeeded = true,
                Severity = AuditSeverity.Information
            });
            _logger.LogInformation(
                "Login user {Username} created successfully.",
                user.Username);

            return new Result<LoginUserDto>(
                LoginUserMapper.ToDto(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while handling {HandlerName}.",
                nameof(CreateLoginUserCommandHandler));

            return new Result<LoginUserDto>(ex);
        }
    }
}