using Application.Features.Othernames;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class OthernamesController : BaseApiController
{
    public OthernamesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all othernames.", Description = "Returns a list of othernames.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of othernames.", typeof(IEnumerable<OthernameDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetOthernamesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a othername by id.", Description = "Returns the othername that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested othername.", typeof(OthernameDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The othername was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetOthernameByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a othername.", Description = "Creates a new othername using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created othername.", typeof(OthernameDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateOthernameCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a othername.", Description = "Updates an existing othername using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated othername.", typeof(OthernameDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The othername was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOthernameCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a othername.", Description = "Deletes the othername that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The othername was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The othername was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteOthernameCommand(id), cancellationToken);
    }
}