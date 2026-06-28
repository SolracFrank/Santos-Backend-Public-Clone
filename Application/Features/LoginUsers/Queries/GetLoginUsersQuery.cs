using Domain.Entities.Dtos;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.LoginUsers.Queries
{
    public class GetLoginUsersQuery : IRequest<Result<List<LoginUserDto>>>
    {

    }
}