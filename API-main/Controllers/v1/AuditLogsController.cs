using Application.Features.AuditLogs.Queries;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class AuditLogsController : BaseApiController
{
    public AuditLogsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Gets recent audit logs.",
        Description = "Returns the most recent audit logs.")]
    [SwaggerResponse(
        StatusCodes.Status200OK,
        "Returns recent audit logs.",
        typeof(IEnumerable<AuditLogDto>))]
    public async Task<IActionResult> Get(
        [FromQuery] int take = 200,
        CancellationToken cancellationToken = default)
    {
        return await SendAsync(
            new GetAuditLogsQuery(take),
            cancellationToken);
    }
}