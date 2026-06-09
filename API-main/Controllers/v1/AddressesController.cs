using Application.Features.Addresses;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class AddressesController : BaseApiController
{
    public AddressesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all addresses.", Description = "Returns a list of addresses.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of addresses.", typeof(IEnumerable<AddressDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetAddresssQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a address by id.", Description = "Returns the address that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested address.", typeof(AddressDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The address was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetAddressByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a address.", Description = "Creates a new address using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created address.", typeof(AddressDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateAddressCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a address.", Description = "Updates an existing address using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated address.", typeof(AddressDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The address was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a address.", Description = "Deletes the address that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The address was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The address was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteAddressCommand(id), cancellationToken);
    }
}