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

public class GetLoginUsersQueryHandler
    : IRequestHandler<GetLoginUsersQuery, Result<List<LoginUserDto>>>
{
    private readonly ILoginUserRepository _loginUserRepository;
    private readonly ILogger<GetLoginUsersQueryHandler> _logger;
    private readonly IPermissionService _permissionService;
    private readonly ICurrentUserService _currentUserService;

    public GetLoginUsersQueryHandler(
        ILoginUserRepository loginUserRepository,
        ILogger<GetLoginUsersQueryHandler> logger,
        IPermissionService permissionService,
        ICurrentUserService currentUserService)
    {
        _loginUserRepository = loginUserRepository;
        _logger = logger;
        _permissionService = permissionService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<LoginUserDto>>> Handle(
        GetLoginUsersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting handler {HandlerName}.",
            nameof(GetLoginUsersQueryHandler));

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
            var users = await _loginUserRepository.GetAllAsync();

            var result = users
                .Select(LoginUserMapper.ToDto)
                .ToList();

            _logger.LogInformation(
                "Retrieved {Count} login users.",
                result.Count);

            return new Result<List<LoginUserDto>>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while handling {HandlerName}.",
                nameof(GetLoginUsersQueryHandler));

            return new Result<List<LoginUserDto>>(ex);
        }
    }
}