using MediatR;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public record DeleteInvoicedetailsByInvoiceCommand(int IdInvoice) : IRequest<Result<Unit>>;