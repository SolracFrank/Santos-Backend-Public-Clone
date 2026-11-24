using Application.Features.PaymentApplies;
using Domain.Entities.Dtos;
using Domain.Entities;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class PaymentAppliesController : BaseApiController
{
    public PaymentAppliesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all paymentapplies.", Description = "Returns a list of paymentapplies.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of paymentapplies.", typeof(IEnumerable<PaymentApplyDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaymentAppliesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a paymentapply by id.", Description = "Returns the paymentapply that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested paymentapply.", typeof(PaymentApplyDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The paymentapply was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaymentApplyByIdQuery(id), cancellationToken);
    }

    [HttpGet("by-payment/{idPayment:int}")]
    [SwaggerOperation(Summary = "Gets payment applies by payment.", Description = "Returns all payment applications associated with a payment.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns payment applications for the provided payment.", typeof(IEnumerable<PaymentApplyDto>))]
    public async Task<IActionResult> GetByPayment(int idPayment, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaymentAppliesByPaymentQuery(idPayment), cancellationToken);
    }

    [HttpGet("by-invoice/{idInvoice:int}")]
    [SwaggerOperation(Summary = "Gets payment applies by invoice.", Description = "Returns all payment applications associated with an invoice.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns payment applications for the provided invoice.", typeof(IEnumerable<PaymentApplyDto>))]
    public async Task<IActionResult> GetByInvoice(int idInvoice, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetPaymentAppliesByInvoiceQuery(idInvoice), cancellationToken);
    }

    [Authorize(Roles = LoginUser.AdministratorRole)]
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a paymentapply.", Description = "Creates a new paymentapply using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created paymentapply.", typeof(PaymentApplyDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreatePaymentApplyCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [Authorize(Roles = LoginUser.AdministratorRole)]
    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a paymentapply.", Description = "Updates an existing paymentapply using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated paymentapply.", typeof(PaymentApplyDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The paymentapply was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaymentApplyCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [Authorize(Roles = LoginUser.AdministratorRole)]
    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a paymentapply.", Description = "Deletes the paymentapply that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The paymentapply was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The paymentapply was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeletePaymentApplyCommand(id), cancellationToken);
    }
}
