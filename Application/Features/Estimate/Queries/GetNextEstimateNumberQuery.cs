using LanguageExt.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Estimate.Queries
{
    public record GetNextEstimateNumberQuery() : IRequest<Result<int>>;
}
