using LanguageExt.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Email;

public sealed record SendEmailCommand : IRequest<Result<Unit>>
{
    public string To { get; init; } = string.Empty;

    public string Subject { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public IFormFile? Pdf { get; init; }

    public List<IFormFile> Files { get; init; } = [];

    public string? EntityName { get; init; }

    public string? EntityId { get; init; }

    public string? DocumentNumber { get; init; }

    public bool IsHtml { get; init; }
}
