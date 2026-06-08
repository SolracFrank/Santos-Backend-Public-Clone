using MediatR;
using LanguageExt.Common;

namespace Application.Features.Invoices;

public record DeleteInvoiceCommand(int Id) : IRequest<Result<Unit>>;