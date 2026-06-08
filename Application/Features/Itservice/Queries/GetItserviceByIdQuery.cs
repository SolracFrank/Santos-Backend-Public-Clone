using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Itservices;

public record GetItserviceByIdQuery(int Id) : IRequest<Result<Itservice?>>;