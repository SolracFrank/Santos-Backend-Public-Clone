using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Users;

public record GetUserByIdQuery(int Id) : IRequest<Result<User?>>;