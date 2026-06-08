using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public record GetEstimatesQuery : IRequest<Result<List<Estimate>>>;