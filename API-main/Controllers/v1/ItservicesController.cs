using Application.Features.Itservices;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class ItservicesController : BaseApiController
{
    public ItservicesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all itservices.", Description = "Returns a list of itservices.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of itservices.", typeof(IEnumerable<ItserviceDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetItservicesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a itservice by id.", Description = "Returns the itservice that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested itservice.", typeof(ItserviceDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The itservice was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetItserviceByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a itservice.", Description = "Creates a new itservice using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created itservice.", typeof(ItserviceDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateItserviceCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a itservice.", Description = "Updates an existing itservice using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated itservice.", typeof(ItserviceDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The itservice was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateItserviceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a itservice.", Description = "Deletes the itservice that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The itservice was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The itservice was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteItserviceCommand(id), cancellationToken);
    }
}