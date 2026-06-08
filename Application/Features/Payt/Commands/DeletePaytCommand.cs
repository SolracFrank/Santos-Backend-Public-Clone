using MediatR;
using LanguageExt.Common;

namespace Application.Features.Payts;

public record DeletePaytCommand(int Id) : IRequest<Result<Unit>>;