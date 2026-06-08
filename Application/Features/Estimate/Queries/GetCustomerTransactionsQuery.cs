using Domain.Entities.Dtos;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public record GetCustomerTransactionsQuery(int IdUser) : IRequest<Result<List<CustomerTransaction>>>;