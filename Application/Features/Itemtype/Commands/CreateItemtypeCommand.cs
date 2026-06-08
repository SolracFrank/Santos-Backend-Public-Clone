using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public record CreateItemtypeCommand(int Iditemt, string Itname) : IRequest<Result<Itemtype>>;