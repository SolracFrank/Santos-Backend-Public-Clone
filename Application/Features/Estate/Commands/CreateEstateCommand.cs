using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Estates;

public record CreateEstateCommand(int Idestate, string Esname) : IRequest<Result<Estate>>;