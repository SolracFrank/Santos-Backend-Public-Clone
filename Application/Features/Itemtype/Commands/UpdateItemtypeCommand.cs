using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public record UpdateItemtypeCommand(int Id, int Iditemt, string Itname) : IRequest<Result<Itemtype>>;