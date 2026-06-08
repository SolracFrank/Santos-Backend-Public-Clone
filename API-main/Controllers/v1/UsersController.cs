using Application.Features.Users;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class UsersController : BaseApiController
{
    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all users.", Description = "Returns a list of users.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of users.", typeof(IEnumerable<UserDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetUsersQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a user by id.", Description = "Returns the user that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested user.", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The user was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetUserByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a user.", Description = "Creates a new user using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created user.", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a user.", Description = "Updates an existing user using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated user.", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The user was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a user.", Description = "Deletes the user that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The user was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteUserCommand(id), cancellationToken);
    }

    [HttpGet("customers-balance")]
    [SwaggerOperation(Summary = "Gets customer balances.", Description = "Returns customer balances filtered and sorted by query parameters.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns customer balances.", typeof(IEnumerable<CustomerBalanceDto>))]
    public async Task<IActionResult> GetCustomersBalance(
        [FromQuery] string orderBy = "",
        [FromQuery] string search = "",
        CancellationToken cancellationToken = default)
    {
        return await SendAsync(new GetCustomersBalanceQuery(orderBy, search), cancellationToken);
    }
}
