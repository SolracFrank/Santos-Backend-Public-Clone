using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public record DeleteEstimatedetailCommand(int Id) : IRequest<Result<Unit>>;