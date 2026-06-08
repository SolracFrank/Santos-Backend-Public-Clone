using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public record GetEstimateByIdQuery(int Id) : IRequest<Result<Estimate?>>;