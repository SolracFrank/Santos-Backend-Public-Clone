using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public record DeleteEstimateCommand(int Id) : IRequest<Result<Unit>>;