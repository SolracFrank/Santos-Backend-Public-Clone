using Application.Features.Paymds;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class PaymdsController : BaseApiController
{
    public PaymdsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all paymds.", Description = "Returns a list of paymds.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of paymds.", typeof(IEnumerable<PaymdDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaymdsQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a paymd by id.", Description = "Returns the paymd that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested paymd.", typeof(PaymdDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The paymd was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaymdByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a paymd.", Description = "Creates a new paymd using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created paymd.", typeof(PaymdDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreatePaymdCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a paymd.", Description = "Updates an existing paymd using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated paymd.", typeof(PaymdDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The paymd was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymdCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a paymd.", Description = "Deletes the paymd that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The paymd was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The paymd was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeletePaymdCommand(id), cancellationToken);
    }
}