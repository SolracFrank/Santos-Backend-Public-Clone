using Application.Features.LoginUsers.Commands;
using Application.Features.LoginUsers.Queries;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class LoginUsersController : BaseApiController
{
    public LoginUsersController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets all login users.",
        Description = "Returns a list of login users.")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Returns a list of login users.",
        typeof(IEnumerable<LoginUserDto>))]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        return await SendAsync(
            new GetLoginUsersQuery(),
            cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(
    Summary = "Gets a login user by id.",
    Description = "Returns the login user that matches the provided id.")]
    [SwaggerResponse(
    StatusCodes.Status200OK,
    "Returns the requested login user.",
    typeof(LoginUserDto))]
    [SwaggerResponse(
    StatusCodes.Status404NotFound,
    "The login user was not found.")]
    public async Task<IActionResult> GetById(
    int id,
    CancellationToken cancellationToken)
    {
        return await SendAsync(
            new GetLoginUserByIdQuery(id),
            cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(
    Summary = "Creates a login user.",
    Description = "Creates a new login user.")]
    [SwaggerResponse(
    StatusCodes.Status200OK,
    "Returns the created login user.",
    typeof(LoginUserDto))]
    [SwaggerResponse(
    StatusCodes.Status400BadRequest,
    "The request body failed validation.")]
    [SwaggerResponse(
    StatusCodes.Status409Conflict,
    "The username already exists.")]
    public async Task<IActionResult> Create(
    [FromBody] CreateLoginUserCommand command,
    CancellationToken cancellationToken)
    {
        return await SendAsync(
            command,
            cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(
    Summary = "Updates a login user.",
    Description = "Updates an existing login user.")]
    [SwaggerResponse(
    StatusCodes.Status200OK,
    "Returns the updated login user.",
    typeof(LoginUserDto))]
    [SwaggerResponse(
    StatusCodes.Status404NotFound,
    "The login user was not found.")]
    [SwaggerResponse(
    StatusCodes.Status409Conflict,
    "The username already exists.")]
    public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateLoginUserCommand command,
    CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}/reset-password")]
    [SwaggerOperation(
    Summary = "Resets a login user's password.",
    Description = "Resets the password for the specified login user.")]
    [SwaggerResponse(
    StatusCodes.Status200OK,
    "Password reset successfully.",
    typeof(LoginUserDto))]
    public async Task<IActionResult> ResetPassword(
    int id,
    [FromBody] ResetPasswordCommand command,
    CancellationToken cancellationToken)
    {
        command.Id = id;

        return await SendAsync(
            command,
            cancellationToken);
    }

    [HttpPut("{id:int}/status")]
    [SwaggerOperation(
    Summary = "Enables or disables a login user.",
    Description = "Updates the active status of a login user.")]
    [SwaggerResponse(
    StatusCodes.Status200OK,
    "Returns the updated login user.",
    typeof(LoginUserDto))]
    public async Task<IActionResult> UpdateStatus(
    int id,
    [FromBody] EnableDisableLoginUserCommand command,
    CancellationToken cancellationToken)
    {
        command.Id = id;

        return await SendAsync(
            command,
            cancellationToken);
    }
}