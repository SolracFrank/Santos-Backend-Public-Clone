using MediatR;
using LanguageExt.Common;

namespace Application.Features.Othernames;

public record DeleteOthernameCommand(int Id) : IRequest<Result<Unit>>;