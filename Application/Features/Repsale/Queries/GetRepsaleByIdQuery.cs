using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Repsales;

public record GetRepsaleByIdQuery(int Id) : IRequest<Result<Repsale?>>;