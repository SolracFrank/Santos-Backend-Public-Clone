using MediatR;
using LanguageExt.Common;

namespace Application.Features.Accounts;

public record DeleteAccountCommand(int Id) : IRequest<Result<Unit>>;