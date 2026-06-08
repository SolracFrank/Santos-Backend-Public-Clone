using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public record GetEstimateExistsQuery(int EstimateNumber) : IRequest<Result<bool>>;