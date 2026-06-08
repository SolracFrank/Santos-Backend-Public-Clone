using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public record GetEstimatedetailsQuery : IRequest<Result<List<Estimatedetail>>>;