using Application.Features.LoginUsers.Mappers;
using Application.Features.LoginUsers.Queries;
using Domain.Common.Security;
using Domain.Entities.Dtos;
using Domain.Exceptions;
using Infrastructure.Security;
using Infrastructure.Services;
using LanguageExt.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.LoginUsers.Handlers;

public class GetLoginUserByIdQueryHandler
    : IRequestHandler<GetLoginUserByIdQuery, Result<LoginUserDto>>
{
    private readonly ILoginUserRepository _loginUserRepository;
    private readonly ILogger<GetLoginUserByIdQueryHandler> _logger;
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUserService;

    public GetLoginUserByIdQueryHandler(
        ILoginUserRepository loginUserRepository,
        ILogger<GetLoginUserByIdQueryHandler> logger,
        IPermissionService permissionService,
        ICurrentUserService currentUserService)
    {
        _loginUserRepository = loginUserRepository;
        _logger = logger;
        _permissionService = permissionService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<LoginUserDto>> Handle(
        GetLoginUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting handler {HandlerName}.",
            nameof(GetLoginUserByIdQueryHandler));

        try
        {
            var canEditUsers = await _permissionService.CanEditUsersAsync(
                _currentUserService.IdLoginUser);

            if (!canEditUsers)
            {
                throw new ForbiddenException(
                    "You do not have permission to view users.",
                    "USER_VIEW_FORBIDDEN");
            }
            var user = await _loginUserRepository.GetByIdAsync(request.Id);

            if (user is null)
            {
                throw new NotFoundException(
                    $"Login user with id {request.Id} was not found.",
                    "LOGINUSER_NOT_FOUND");
            }

            var result = LoginUserMapper.ToDto(user);

            _logger.LogInformation(
                "Login user {Id} retrieved successfully.",
                request.Id);

            return new Result<LoginUserDto>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while handling {HandlerName}.",
                nameof(GetLoginUserByIdQueryHandler));

            return new Result<LoginUserDto>(ex);
        }
    }
}