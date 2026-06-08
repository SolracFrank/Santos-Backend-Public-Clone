using Application.Features.Customermessages;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class CustomermessagesController : BaseApiController
{
    public CustomermessagesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all customermessages.", Description = "Returns a list of customermessages.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of customermessages.", typeof(IEnumerable<CustomermessageDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetCustomermessagesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a customermessage by id.", Description = "Returns the customermessage that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested customermessage.", typeof(CustomermessageDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The customermessage was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetCustomermessageByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a customermessage.", Description = "Creates a new customermessage using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created customermessage.", typeof(CustomermessageDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateCustomermessageCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a customermessage.", Description = "Updates an existing customermessage using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated customermessage.", typeof(CustomermessageDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The customermessage was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomermessageCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a customermessage.", Description = "Deletes the customermessage that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The customermessage was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The customermessage was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteCustomermessageCommand(id), cancellationToken);
    }
}