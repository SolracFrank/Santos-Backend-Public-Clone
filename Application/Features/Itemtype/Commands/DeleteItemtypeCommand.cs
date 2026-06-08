using MediatR;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public record DeleteItemtypeCommand(int Id) : IRequest<Result<Unit>>;