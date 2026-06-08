using Application.Features.Repsales;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class RepsalesController : BaseApiController
{
    public RepsalesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all repsales.", Description = "Returns a list of repsales.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of repsales.", typeof(IEnumerable<RepsaleDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetRepsalesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a repsale by id.", Description = "Returns the repsale that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested repsale.", typeof(RepsaleDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The repsale was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetRepsaleByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a repsale.", Description = "Creates a new repsale using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created repsale.", typeof(RepsaleDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateRepsaleCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a repsale.", Description = "Updates an existing repsale using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated repsale.", typeof(RepsaleDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The repsale was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRepsaleCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a repsale.", Description = "Deletes the repsale that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The repsale was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The repsale was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteRepsaleCommand(id), cancellationToken);
    }
}