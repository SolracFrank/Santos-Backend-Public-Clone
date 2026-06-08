using Application.Features.Estimatedetails;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class EstimatedetailsController : BaseApiController
{
    public EstimatedetailsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all estimatedetails.", Description = "Returns a list of estimatedetails.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of estimatedetails.", typeof(IEnumerable<EstimatedetailDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEstimatedetailsQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a estimatedetail by id.", Description = "Returns the estimatedetail that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested estimatedetail.", typeof(EstimatedetailDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The estimatedetail was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEstimatedetailByIdQuery(id), cancellationToken);
    }

    [HttpGet("by-estimate/{idEstimate:int}")]
    [SwaggerOperation(Summary = "Gets estimate details by estimate.", Description = "Returns all details associated with an estimate.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns estimate details for the provided estimate.", typeof(IEnumerable<EstimatedetailDto>))]
    public async Task<IActionResult> GetByEstimate(int idEstimate, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEstimatedetailsByEstimateQuery(idEstimate), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a estimatedetail.", Description = "Creates a new estimatedetail using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created estimatedetail.", typeof(EstimatedetailDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateEstimatedetailCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a estimatedetail.", Description = "Updates an existing estimatedetail using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated estimatedetail.", typeof(EstimatedetailDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The estimatedetail was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEstimatedetailCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a estimatedetail.", Description = "Deletes the estimatedetail that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The estimatedetail was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The estimatedetail was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteEstimatedetailCommand(id), cancellationToken);
    }

    [HttpDelete("by-estimate/{idEstimate:int}")]
    [SwaggerOperation(Summary = "Deletes estimate details by estimate.", Description = "Deletes all details associated with an estimate.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The estimate details were deleted.")]
    public async Task<IActionResult> DeleteByEstimate(int idEstimate, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteEstimatedetailsByEstimateCommand(idEstimate), cancellationToken);
    }
}
