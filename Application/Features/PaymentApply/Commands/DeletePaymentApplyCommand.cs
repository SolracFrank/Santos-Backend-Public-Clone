using MediatR;
using LanguageExt.Common;

namespace Application.Features.PaymentApplies;

public record DeletePaymentApplyCommand(int Id) : IRequest<Result<Unit>>;