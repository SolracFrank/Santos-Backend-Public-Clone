using Application.Features.Payts;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class PaytsController : BaseApiController
{
    public PaytsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all payts.", Description = "Returns a list of payts.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of payts.", typeof(IEnumerable<PaytDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaytsQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a payt by id.", Description = "Returns the payt that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested payt.", typeof(PaytDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The payt was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaytByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a payt.", Description = "Creates a new payt using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created payt.", typeof(PaytDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreatePaytCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a payt.", Description = "Updates an existing payt using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated payt.", typeof(PaytDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The payt was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaytCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a payt.", Description = "Deletes the payt that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The payt was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The payt was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeletePaytCommand(id), cancellationToken);
    }
}