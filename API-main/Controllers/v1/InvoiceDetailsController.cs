using Application.Features.Invoicedetails;
using Domain.Entities.Dtos;
using Domain.Entities;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class InvoiceDetailsController : BaseApiController
{
    public InvoiceDetailsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all invoicedetails.", Description = "Returns a list of invoicedetails.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of invoicedetails.", typeof(IEnumerable<InvoicedetailDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetInvoicedetailsQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a invoicedetail by id.", Description = "Returns the invoicedetail that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested invoicedetail.", typeof(InvoicedetailDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The invoicedetail was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetInvoicedetailByIdQuery(id), cancellationToken);
    }

    [HttpGet("by-invoice/{idInvoice:int}")]
    [SwaggerOperation(Summary = "Gets invoice details by invoice.", Description = "Returns all details associated with an invoice.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns invoice details for the provided invoice.", typeof(IEnumerable<InvoicedetailDto>))]
    public async Task<IActionResult> GetByInvoice(int idInvoice, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetInvoicedetailsByInvoiceQuery(idInvoice), cancellationToken);
    }

    [Authorize(Roles = LoginUser.AdministratorRole)]
    [HttpPost]
    [SwaggerOperation(Summary = "Creates a invoicedetail.", Description = "Creates a new invoicedetail using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created invoicedetail.", typeof(InvoicedetailDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateInvoicedetailCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [Authorize(Roles = LoginUser.AdministratorRole)]
    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a invoicedetail.", Description = "Updates an existing invoicedetail using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated invoicedetail.", typeof(InvoicedetailDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The invoicedetail was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoicedetailCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [Authorize(Roles = LoginUser.AdministratorRole)]
    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a invoicedetail.", Description = "Deletes the invoicedetail that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The invoicedetail was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The invoicedetail was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteInvoicedetailCommand(id), cancellationToken);
    }

    [Authorize(Roles = LoginUser.AdministratorRole)]
    [HttpDelete("by-invoice/{idInvoice:int}")]
    [SwaggerOperation(Summary = "Deletes invoice details by invoice.", Description = "Deletes all details associated with an invoice.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The invoice details were deleted.")]
    public async Task<IActionResult> DeleteByInvoice(int idInvoice, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteInvoicedetailsByInvoiceCommand(idInvoice), cancellationToken);
    }
}
