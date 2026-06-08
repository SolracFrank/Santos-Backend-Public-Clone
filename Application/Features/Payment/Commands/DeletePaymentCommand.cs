using MediatR;
using LanguageExt.Common;

namespace Application.Features.Payments;

public record DeletePaymentCommand(int Id) : IRequest<Result<Unit>>;