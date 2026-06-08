using Application.Features.Accounts;
using Domain.Entities.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class AccountsController : BaseApiController
{
    public AccountsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all accounts.", Description = "Returns a list of accounts.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of accounts.", typeof(IEnumerable<AccountDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetAccountsQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a account by id.", Description = "Returns the account that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested account.", typeof(AccountDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The account was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetAccountByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a account.", Description = "Creates a new account using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created account.", typeof(AccountDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateAccountCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a account.", Description = "Updates an existing account using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated account.", typeof(AccountDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The account was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a account.", Description = "Deletes the account that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The account was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The account was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteAccountCommand(id), cancellationToken);
    }
}