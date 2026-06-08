using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Customers;

public record GetCustomersQuery : IRequest<Result<List<Customer>>>;