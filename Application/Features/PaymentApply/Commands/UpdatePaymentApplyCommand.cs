using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public record UpdatePaymentApplyCommand(int Id, int Idpaymentapply, int Idpayment, int Idinvoice, decimal AmountApplied) : IRequest<Result<PaymentApply>>;