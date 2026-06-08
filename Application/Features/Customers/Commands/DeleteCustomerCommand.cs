using MediatR;
using LanguageExt.Common;

namespace Application.Features.Customers;

public record DeleteCustomerCommand(int Id) : IRequest<Result<Unit>>;