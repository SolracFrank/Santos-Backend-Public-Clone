using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Itemtypes;

public record GetItemtypeByIdQuery(int Id) : IRequest<Result<Itemtype?>>;