using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Payments;

public record GetPaymentByIdQuery(int Id) : IRequest<Result<Payment?>>;