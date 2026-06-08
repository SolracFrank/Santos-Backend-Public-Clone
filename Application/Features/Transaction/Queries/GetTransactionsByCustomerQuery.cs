using Domain.Entities.Dtos;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Transaction;

public record GetTransactionsByCustomerQuery(int IdUser) : IRequest<Result<List<CustomerTransaction>>>;