using MediatR;
using LanguageExt.Common;

namespace Application.Features.Terms;

public record DeleteTermCommand(int Id) : IRequest<Result<Unit>>;