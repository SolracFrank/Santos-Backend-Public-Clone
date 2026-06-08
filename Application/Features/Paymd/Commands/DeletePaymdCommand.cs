using MediatR;
using LanguageExt.Common;

namespace Application.Features.Paymds;

public record DeletePaymdCommand(int Id) : IRequest<Result<Unit>>;