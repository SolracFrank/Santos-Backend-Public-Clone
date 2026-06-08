using Application.Features.Estimates;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class EstimatesController : BaseApiController
{
    public EstimatesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all estimates.", Description = "Returns a list of estimates.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of estimates.", typeof(IEnumerable<EstimateDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEstimatesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a estimate by id.", Description = "Returns the estimate that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested estimate.", typeof(EstimateDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The estimate was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEstimateByIdQuery(id), cancellationToken);
    }

    [HttpGet("customer-transactions/{idUser:int}")]
    [SwaggerOperation(Summary = "Gets customer transactions.", Description = "Returns estimate transactions for the provided user id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns customer transactions.", typeof(IEnumerable<CustomerTransaction>))]
    public async Task<IActionResult> GetCustomerTransactions(int idUser, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetCustomerTransactionsQuery(idUser), cancellationToken);
    }

    [HttpGet("exists/{estimateNumber:int}")]
    [SwaggerOperation(Summary = "Checks whether an estimate exists.", Description = "Returns true when an estimate number already exists.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns whether the estimate exists.", typeof(bool))]
    public async Task<IActionResult> Exists(int estimateNumber, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEstimateExistsQuery(estimateNumber), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a estimate.", Description = "Creates a new estimate using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created estimate.", typeof(EstimateDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateEstimateCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a estimate.", Description = "Updates an existing estimate using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated estimate.", typeof(EstimateDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The estimate was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEstimateCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a estimate.", Description = "Deletes the estimate that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The estimate was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The estimate was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteEstimateCommand(id), cancellationToken);
    }
}
