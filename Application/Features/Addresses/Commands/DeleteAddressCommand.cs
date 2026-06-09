using MediatR;
using LanguageExt.Common;

namespace Application.Features.Addresses;

public record DeleteAddressCommand(int Id) : IRequest<Result<Unit>>;