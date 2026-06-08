using MediatR;
using Microsoft.AspNetCore.Http;
using LanguageExt.Common;

namespace Application.Features.Email;

public record SendPdfEmailCommand(
    string To,
    string Subject,
    string Message,
    IFormFile Pdf) : IRequest<Result<Unit>>;