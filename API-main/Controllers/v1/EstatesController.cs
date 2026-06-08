using Application.Features.Estates;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class EstatesController : BaseApiController
{
    public EstatesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all estates.", Description = "Returns a list of estates.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of estates.", typeof(IEnumerable<EstateDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEstatesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a estate by id.", Description = "Returns the estate that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested estate.", typeof(EstateDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The estate was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEstateByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a estate.", Description = "Creates a new estate using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created estate.", typeof(EstateDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateEstateCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a estate.", Description = "Updates an existing estate using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated estate.", typeof(EstateDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The estate was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEstateCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a estate.", Description = "Deletes the estate that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The estate was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The estate was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteEstateCommand(id), cancellationToken);
    }
}