using MediatR;
using System.Collections.Generic;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public record GetInvoicedetailsByInvoiceQuery(int IdInvoice) : IRequest<Result<List<Invoicedetail>>>;