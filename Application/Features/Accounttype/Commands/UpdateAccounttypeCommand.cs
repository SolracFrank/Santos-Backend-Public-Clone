using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public record UpdateAccounttypeCommand(int Id, int Idaccountt, string Atype) : IRequest<Result<Accounttype>>;