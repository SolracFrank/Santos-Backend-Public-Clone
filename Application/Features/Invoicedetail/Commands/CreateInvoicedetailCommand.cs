using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public record CreateInvoicedetailCommand(int Idinvoicedetail, int Idinvoice, int? Iditem, string? Itemname, string? Description, decimal Qty, decimal Cost, decimal Total) : IRequest<Result<Invoicedetail>>;