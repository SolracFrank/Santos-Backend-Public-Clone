using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Accounts;

public record GetAccountByIdQuery(int Id) : IRequest<Result<Account?>>;