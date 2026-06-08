using MediatR;
using System.Collections.Generic;
using Domain.Entities;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public record GetEstimatedetailsByEstimateQuery(int IdEstimate) : IRequest<Result<List<Estimatedetail>>>;