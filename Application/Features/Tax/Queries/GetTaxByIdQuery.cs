using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Taxes;

public record GetTaxByIdQuery(int Id) : IRequest<Result<Tax?>>;