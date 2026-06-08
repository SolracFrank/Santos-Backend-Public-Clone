using Application.Features.Accounttypes;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class AccounttypesController : BaseApiController
{
    public AccounttypesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all accounttypes.", Description = "Returns a list of accounttypes.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of accounttypes.", typeof(IEnumerable<AccounttypeDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetAccounttypesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a accounttype by id.", Description = "Returns the accounttype that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested accounttype.", typeof(AccounttypeDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The accounttype was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetAccounttypeByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a accounttype.", Description = "Creates a new accounttype using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created accounttype.", typeof(AccounttypeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateAccounttypeCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a accounttype.", Description = "Updates an existing accounttype using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated accounttype.", typeof(AccounttypeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The accounttype was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAccounttypeCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a accounttype.", Description = "Deletes the accounttype that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The accounttype was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The accounttype was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteAccounttypeCommand(id), cancellationToken);
    }
}