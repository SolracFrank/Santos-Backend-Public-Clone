using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public record CreateTaxCommand(int Idtax, string Tname) : IRequest<Result<Tax>>;