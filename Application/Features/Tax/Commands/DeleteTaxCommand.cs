using MediatR;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public record DeleteTaxCommand(int Id) : IRequest<Result<Unit>>;