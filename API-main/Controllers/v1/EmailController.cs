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
    [SwaggerOperation(Summary = "Sends a PDF email.", Description = "Sends an email with a PDF attachment using multipart form data.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The email was sent.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request failed validation.")]
    public async Task<IActionResult> SendPdfEmail([FromForm] SendPdfEmailCommand command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Ok(new { message = "Correo enviado correctamente." });
    }
}
