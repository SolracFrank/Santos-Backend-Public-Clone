using Application.Features.Taxes;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class TaxController : BaseApiController
{
    public TaxController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all taxs.", Description = "Returns a list of taxs.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of taxs.", typeof(IEnumerable<TaxDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetTaxsQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a tax by id.", Description = "Returns the tax that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested tax.", typeof(TaxDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The tax was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetTaxByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a tax.", Description = "Creates a new tax using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created tax.", typeof(TaxDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateTaxCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a tax.", Description = "Updates an existing tax using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated tax.", typeof(TaxDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The tax was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaxCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a tax.", Description = "Deletes the tax that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The tax was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The tax was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteTaxCommand(id), cancellationToken);
    }
}