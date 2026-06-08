using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estates;

public record GetEstateByIdQuery(int Id) : IRequest<Result<Estate?>>;