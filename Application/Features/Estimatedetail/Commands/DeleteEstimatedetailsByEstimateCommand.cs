using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public record DeleteEstimatedetailsByEstimateCommand(int IdEstimate) : IRequest<Result<Unit>>;