using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Payments;

public record CreatePaymentCommand(int Idpayment, int Paymentnumber, DateTime Paymentdate, decimal Amount, string? Memo, int? Idusers, int? Idpaymd, int? Idaccount) : IRequest<Result<Payment>>;