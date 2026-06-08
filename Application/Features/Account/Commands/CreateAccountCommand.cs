using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Accounts;

public record CreateAccountCommand(int Idaccount, string Aname, string? Adescription, string? Abankno, string? Anotes, int? Idtax, int? Idaccountt) : IRequest<Result<Account>>;