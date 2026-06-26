using Application.Features.LoginUsers.Commands;
using Application.Features.LoginUsers.Mappers;
using Domain.Common.Security;
using Domain.Entities.Dtos;
using Domain.Exceptions;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.LoginUsers.Handlers;

public class EnableDisableLoginUserCommandHandler
    : IRequestHandler<EnableDisableLoginUserCommand, Result<LoginUserDto>>
{
    private readonly ILoginUserRepository _loginUserRepository;
    private readonly ILogger<EnableDisableLoginUserCommandHandler> _logger;
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUserService;

    public EnableDisableLoginUserCommandHandler(
        ILoginUserRepository loginUserRepository,
        ILogger<EnableDisableLoginUserCommandHandler> logger,
        IPermissionService permissionService,
        ICurrentUserService currentUserService)
    {
        _loginUserRepository = loginUserRepository;
        _logger = logger;
        _permissionService = permissionService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<LoginUserDto>> Handle(
        EnableDisableLoginUserCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting handler {HandlerName}.",
            nameof(EnableDisableLoginUserCommandHandler));

        try
        {
            var canDisableUsers = await _permissionService.CanDisableUsersAsync(
                _currentUserService.IdLoginUser);

            if (!canDisableUsers)
            {
                throw new ForbiddenException(
                    "You do not have permission to enable or disable users.",
                    "USER_STATUS_FORBIDDEN");
            }
            var user = await _loginUserRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                throw new NotFoundException(
                    $"Login user with id {request.Id} was not found.",
                    "LOGINUSER_NOT_FOUND");
            }

            user.IsActive = request.IsActive;

            await _loginUserRepository.UpdateAsync(user);

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
                nameof(EnableDisableLoginUserCommandHandler));

            return new Result<LoginUserDto>(ex);
        }
    }
}