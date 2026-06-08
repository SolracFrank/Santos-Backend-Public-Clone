using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public record GetEstimatedetailByIdQuery(int Id) : IRequest<Result<Estimatedetail?>>;