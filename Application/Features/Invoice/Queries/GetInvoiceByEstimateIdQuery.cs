using LanguageExt.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Invoice.Queries
{
    public record GetInvoiceByEstimateIdQuery(int IdEstimate)
    : IRequest<Result<Domain.Entities.Invoice>>;
}
