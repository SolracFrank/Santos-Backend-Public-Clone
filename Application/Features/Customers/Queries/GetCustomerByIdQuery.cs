using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Customers;

public record GetCustomerByIdQuery(int Id) : IRequest<Result<Customer?>>;