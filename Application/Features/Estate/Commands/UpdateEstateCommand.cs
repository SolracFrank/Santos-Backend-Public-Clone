using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estates;

public record UpdateEstateCommand(int Id, int Idestate, string Esname) : IRequest<Result<Estate>>;