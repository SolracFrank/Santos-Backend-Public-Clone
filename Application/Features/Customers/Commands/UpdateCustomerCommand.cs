using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Customers;

public record UpdateCustomerCommand(int Id, int Idcustom, string Cname, int? Idcustomfk) : IRequest<Result<Customer>>;