using Application.Features.Itemtypes;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class ItemtypesController : BaseApiController
{
    public ItemtypesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all itemtypes.", Description = "Returns a list of itemtypes.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of itemtypes.", typeof(IEnumerable<ItemtypeDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetItemtypesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a itemtype by id.", Description = "Returns the itemtype that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested itemtype.", typeof(ItemtypeDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The itemtype was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetItemtypeByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a itemtype.", Description = "Creates a new itemtype using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created itemtype.", typeof(ItemtypeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateItemtypeCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a itemtype.", Description = "Updates an existing itemtype using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated itemtype.", typeof(ItemtypeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The itemtype was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItemtypeCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a itemtype.", Description = "Deletes the itemtype that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The itemtype was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The itemtype was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteItemtypeCommand(id), cancellationToken);
    }
}