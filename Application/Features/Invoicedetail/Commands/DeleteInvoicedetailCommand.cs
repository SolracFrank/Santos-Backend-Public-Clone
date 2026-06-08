using MediatR;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public record DeleteInvoicedetailCommand(int Id) : IRequest<Result<Unit>>;