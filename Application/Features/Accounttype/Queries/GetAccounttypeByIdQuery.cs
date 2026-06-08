using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public record GetAccounttypeByIdQuery(int Id) : IRequest<Result<Accounttype?>>;