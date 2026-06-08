using MediatR;
using LanguageExt.Common;

namespace Application.Features.Users;

public record DeleteUserCommand(int Id) : IRequest<Result<Unit>>;