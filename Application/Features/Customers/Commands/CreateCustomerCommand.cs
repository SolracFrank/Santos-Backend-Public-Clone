using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Customers;

public record CreateCustomerCommand(int Idcustom, string Cname, int? Idcustomfk) : IRequest<Result<Customer>>;