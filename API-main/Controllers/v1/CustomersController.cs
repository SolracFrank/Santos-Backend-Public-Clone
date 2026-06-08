using Application.Features.Customers;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class CustomersController : BaseApiController
{
    public CustomersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all customers.", Description = "Returns a list of customers.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of customers.", typeof(IEnumerable<CustomerDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetCustomersQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a customer by id.", Description = "Returns the customer that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested customer.", typeof(CustomerDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The customer was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetCustomerByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a customer.", Description = "Creates a new customer using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created customer.", typeof(CustomerDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a customer.", Description = "Updates an existing customer using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated customer.", typeof(CustomerDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The customer was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a customer.", Description = "Deletes the customer that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The customer was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The customer was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteCustomerCommand(id), cancellationToken);
    }
}