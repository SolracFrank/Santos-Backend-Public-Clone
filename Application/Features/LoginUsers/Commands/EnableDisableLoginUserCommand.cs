using Domain.Entities.Dtos;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.LoginUsers.Commands;

public class EnableDisableLoginUserCommand : IRequest<Result<LoginUserDto>>
{
    public int Id { get; set; }

    public bool IsActive { get; set; }
}