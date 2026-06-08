using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Payments;

public record UpdatePaymentCommand(int Id, int Idpayment, int Paymentnumber, DateTime Paymentdate, decimal Amount, string? Memo, int? Idusers, int? Idpaymd, int? Idaccount) : IRequest<Result<Payment>>;