using Application.Features.Payments;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class PaymentsController : BaseApiController
{
    public PaymentsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all payments.", Description = "Returns a list of payments.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of payments.", typeof(IEnumerable<PaymentDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaymentsQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a payment by id.", Description = "Returns the payment that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested payment.", typeof(PaymentDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The payment was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaymentByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a payment.", Description = "Creates a new payment using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created payment.", typeof(PaymentDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a payment.", Description = "Updates an existing payment using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated payment.", typeof(PaymentDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The payment was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a payment.", Description = "Deletes the payment that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The payment was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The payment was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new DeletePaymentCommand(id), cancellationToken);
    }
}