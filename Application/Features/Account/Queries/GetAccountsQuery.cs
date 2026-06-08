using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Accounts;

public record GetAccountsQuery : IRequest<Result<List<Account>>>;