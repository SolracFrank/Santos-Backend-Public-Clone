using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Accounttypes;

public record CreateAccounttypeCommand(int Idaccountt, string Atype) : IRequest<Result<Accounttype>>;