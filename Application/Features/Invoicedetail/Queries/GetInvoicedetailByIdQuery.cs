using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public record GetInvoicedetailByIdQuery(int Id) : IRequest<Result<Invoicedetail?>>;