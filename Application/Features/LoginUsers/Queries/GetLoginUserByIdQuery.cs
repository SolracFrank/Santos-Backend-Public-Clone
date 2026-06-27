using Domain.Entities.Dtos;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.LoginUsers.Queries
{
    public class GetLoginUserByIdQuery : IRequest<Result<LoginUserDto>>
    {
        public int Id { get; }

        public GetLoginUserByIdQuery(int id)
        {
            Id = id;
        }
    }
}