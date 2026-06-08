using Application.Features.Jobinfos;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class JobinfosController : BaseApiController
{
    public JobinfosController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all jobinfos.", Description = "Returns a list of jobinfos.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of jobinfos.", typeof(IEnumerable<JobinfoDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetJobinfosQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a jobinfo by id.", Description = "Returns the jobinfo that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested jobinfo.", typeof(JobinfoDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The jobinfo was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetJobinfoByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a jobinfo.", Description = "Creates a new jobinfo using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created jobinfo.", typeof(JobinfoDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateJobinfoCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a jobinfo.", Description = "Updates an existing jobinfo using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated jobinfo.", typeof(JobinfoDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The jobinfo was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateJobinfoCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a jobinfo.", Description = "Deletes the jobinfo that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The jobinfo was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The jobinfo was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteJobinfoCommand(id), cancellationToken);
    }
}