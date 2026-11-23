using Domain.Entities;
using LanguageExt.Common;
using MediatR;

public record UpdatePaymentCommand(
    int Id,
    int Idpayment,
    int Paymentnumber,
    DateTime Paymentdate,
    decimal Amount,
    string? Memo,
    string? Reference,
    int? Idusers,
    int? Idpaymd,
    int? Idaccount
) : IRequest<Result<Payment>>;