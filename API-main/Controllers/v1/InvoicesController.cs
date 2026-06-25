using Application.Features.Invoices;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Application.Features.Invoice.Queries;

namespace EsApi.Controllers.v1;

public class InvoicesController : BaseApiController
{
    public InvoicesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all invoices.", Description = "Returns a list of invoices.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of invoices.", typeof(IEnumerable<InvoiceDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetInvoicesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a invoice by id.", Description = "Returns the invoice that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested invoice.", typeof(InvoiceDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The invoice was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetInvoiceByIdQuery(id), cancellationToken);
    }

    [HttpGet("by-estimate/{idEstimate:int}")]
    [SwaggerOperation(
    Summary = "Gets an invoice by estimate id.",
    Description = "Returns the invoice created from the provided estimate id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the invoice.", typeof(InvoiceDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The invoice was not found.")]
    public async Task<IActionResult> GetByEstimateId(
    int idEstimate,
    CancellationToken cancellationToken)
    {
        return await SendAsync(
            new GetInvoiceByEstimateIdQuery(idEstimate),
            cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a invoice.", Description = "Creates a new invoice using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created invoice.", typeof(InvoiceDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a invoice.", Description = "Updates an existing invoice using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated invoice.", typeof(InvoiceDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The invoice was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a invoice.", Description = "Deletes the invoice that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The invoice was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The invoice was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteInvoiceCommand(id), cancellationToken);
    }

    [HttpGet("next-number")]
    public async Task<IActionResult> GetNextNumber(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetNextInvoiceNumberQuery(), cancellationToken);
    }
}