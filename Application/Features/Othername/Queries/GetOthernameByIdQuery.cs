using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Othernames;

public record GetOthernameByIdQuery(int Id) : IRequest<Result<Othername?>>;