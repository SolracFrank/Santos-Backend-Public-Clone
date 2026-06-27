using Domain.Entities.Dtos;
using LanguageExt.Common;
using MediatR;

namespace Application.Features.LoginUsers.Commands;

public class ResetPasswordCommand : IRequest<Result<LoginUserDto>>
{
    public int Id { get; set; }

    public string NewPassword { get; set; } = null!;
}