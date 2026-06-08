using MediatR;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public record DeleteAccounttypeCommand(int Id) : IRequest<Result<Unit>>;