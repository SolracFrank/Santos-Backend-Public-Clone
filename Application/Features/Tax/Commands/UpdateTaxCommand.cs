using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public record UpdateTaxCommand(int Id, int Idtax, string Tname) : IRequest<Result<Tax>>;