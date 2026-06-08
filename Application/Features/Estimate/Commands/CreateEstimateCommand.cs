using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estimates;

public record CreateEstimateCommand(int Idestimate, int Estimatenumber, DateTime Estimatedate, DateTime? Expdate, DateTime? Datetobecompleted, DateTime? Dateordered, string? Memo, decimal Subtotal, decimal Markup, decimal Total, bool? Activeestimate, decimal Openbalance, int? Idusers, int? Idterms, int? Idrepsales, int? Idcustomermessage, string? Customermessagetext, bool IsCopy, int? CopiedFromEstimateId) : IRequest<Result<Estimate>>;