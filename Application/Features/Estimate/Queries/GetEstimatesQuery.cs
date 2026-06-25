using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public record GetEstimatesQuery()
    : IRequest<Result<List<Domain.Entities.Estimate>>>;