using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public record CreatePaymentApplyCommand(int Idpaymentapply, int Idpayment, int Idinvoice, decimal AmountApplied) : IRequest<Result<PaymentApply>>;