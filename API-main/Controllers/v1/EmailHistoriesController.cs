using Application.Features.EmailHistories;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EsApi.Controllers.v1;

[ApiVersion("1.0")]
public class EmailHistoriesController : BaseApiController
{
    public EmailHistoriesController(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet("customer/{idUser}")]
    public async Task<IActionResult> GetByCustomer(int idUser)
    {
        var result = await Mediator.Send(
            new GetEmailHistoryByCustomerQuery(idUser));

        return result.Match<IActionResult>(
            data => Ok(data),
            ex => BadRequest(ex.Message));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateEmailHistoryCommand command)
    {
        var result = await Mediator.Send(command);

        return result.Match<IActionResult>(
            data => Ok(data),
            ex => BadRequest(ex.Message));
    }
}