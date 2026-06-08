using Domain.Entities.Dtos;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Users;

public record GetCustomersBalanceQuery(string OrderBy, string Search) : IRequest<Result<List<CustomerBalanceDto>>>;