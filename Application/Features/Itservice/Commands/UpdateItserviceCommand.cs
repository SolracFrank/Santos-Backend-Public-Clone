using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Itservices;

public record UpdateItserviceCommand(int Id, int Iditem, string Iname, string? Idescription, string? Irate, string? Idescpurchase, string? Icost, string? Idescsales, string? Isales, int? Iditemt, int? Idaccountp, int? Idaccounts, int? Idaccountn, int? Idpaymd, int? Iditemfk) : IRequest<Result<Itservice>>;