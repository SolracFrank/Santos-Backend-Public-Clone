using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Terms;

public record CreateTermCommand(int Idterms, string Tname, int? Tnets, decimal? Tperccs, int? Tnetd, int? Tdags, decimal? Tpercd) : IRequest<Result<Term>>;