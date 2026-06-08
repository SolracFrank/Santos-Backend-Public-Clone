using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Paymds;

public record CreatePaymdCommand(int Idpaymd, string Pmdname, int? Idpayt) : IRequest<Result<Paymd>>;