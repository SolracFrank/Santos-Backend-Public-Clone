using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estimatedetails;

public record UpdateEstimatedetailCommand(int Id, int Idestimatedetail, int Idestimate, int? Iditem, string? Itemname, string? Description, decimal Qty, decimal Cost, decimal Total) : IRequest<Result<Estimatedetail>>;