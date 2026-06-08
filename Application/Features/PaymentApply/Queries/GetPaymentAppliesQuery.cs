using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public record GetPaymentAppliesQuery : IRequest<Result<List<PaymentApply>>>;