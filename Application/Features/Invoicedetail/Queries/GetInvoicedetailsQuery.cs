using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Invoicedetails;

public record GetInvoicedetailsQuery : IRequest<Result<List<Invoicedetail>>>;