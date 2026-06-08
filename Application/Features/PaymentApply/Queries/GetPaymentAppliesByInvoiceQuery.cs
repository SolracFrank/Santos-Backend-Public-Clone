using MediatR;
using System.Collections.Generic;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public record GetPaymentAppliesByInvoiceQuery(int IdInvoice) : IRequest<Result<List<PaymentApply>>>;