using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Users;

public record GetUsersQuery : IRequest<Result<List<User>>>;