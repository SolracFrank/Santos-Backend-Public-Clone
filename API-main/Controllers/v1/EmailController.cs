using Application.Features.Email;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class EmailController : BaseApiController
{
    public EmailController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("send-pdf")]
    [Consumes("multipart/form-data")]
    [SwaggerOperation(Summary = "Sends an email with attachments.", Description = "Sends an email with an optional main PDF and optional additional validated attachments using multipart form data.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The email was sent.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request failed validation.")]
    public Task<IActionResult> SendEmail([FromForm] SendEmailCommand command, CancellationToken cancellationToken)
    {
        return SendOkAsync(command, cancellationToken);
    }
}
