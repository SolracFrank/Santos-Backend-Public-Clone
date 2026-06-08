using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public record GetPaymentApplyByIdQuery(int Id) : IRequest<Result<PaymentApply?>>;