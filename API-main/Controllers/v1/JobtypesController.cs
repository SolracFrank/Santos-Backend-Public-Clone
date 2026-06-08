using Application.Features.Jobtypes;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class JobtypesController : BaseApiController
{
    public JobtypesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all jobtypes.", Description = "Returns a list of jobtypes.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of jobtypes.", typeof(IEnumerable<JobtypeDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetJobtypesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a jobtype by id.", Description = "Returns the jobtype that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested jobtype.", typeof(JobtypeDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The jobtype was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetJobtypeByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a jobtype.", Description = "Creates a new jobtype using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created jobtype.", typeof(JobtypeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateJobtypeCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a jobtype.", Description = "Updates an existing jobtype using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated jobtype.", typeof(JobtypeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The jobtype was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateJobtypeCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a jobtype.", Description = "Deletes the jobtype that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The jobtype was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The jobtype was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteJobtypeCommand(id), cancellationToken);
    }
}