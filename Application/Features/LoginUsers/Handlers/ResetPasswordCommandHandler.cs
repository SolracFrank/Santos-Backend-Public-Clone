using Application.Features.LoginUsers.Commands;
using Application.Features.LoginUsers.Mappers;
using Domain.Common.Security;
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

    public ResetPasswordCommandHandler(
        ILoginUserRepository loginUserRepository,
        IPasswordHasher passwordHasher,
        ILogger<ResetPasswordCommandHandler> logger,
        IPermissionService permissionService,
        ICurrentUserService currentUserService)
    {
        _loginUserRepository = loginUserRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
        _permissionService = permissionService;
        _currentUserService = currentUserService;
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

            user.PasswordHash =
                _passwordHasher.HashPassword(request.NewPassword);

            user.SecurityStamp = Guid.NewGuid();

            await _loginUserRepository.UpdateAsync(user);

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