using Application.Features.Terms;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class TermsController : BaseApiController
{
    public TermsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all terms.", Description = "Returns a list of terms.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of terms.", typeof(IEnumerable<TermDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetTermsQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a term by id.", Description = "Returns the term that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested term.", typeof(TermDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The term was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetTermByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a term.", Description = "Creates a new term using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created term.", typeof(TermDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateTermCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a term.", Description = "Updates an existing term using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated term.", typeof(TermDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The term was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTermCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a term.", Description = "Deletes the term that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The term was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The term was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteTermCommand(id), cancellationToken);
    }
}