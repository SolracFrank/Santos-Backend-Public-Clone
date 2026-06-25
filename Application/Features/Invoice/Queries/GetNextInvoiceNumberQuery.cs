using LanguageExt.Common;
using MediatR;

namespace Application.Features.Invoices;

public record GetNextInvoiceNumberQuery()
    : IRequest<Result<int>>;