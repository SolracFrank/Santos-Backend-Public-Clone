using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Terms;

public record GetTermByIdQuery(int Id) : IRequest<Result<Term?>>;