using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Users;

public record CreateUserCommand(int Idusers, string Ucustomer, string? Ubalance, DateTime? Udate, int? Idaddress, int? Idterms, int? Idcustomer, int? Idrepsales, int? Idjobinfo) : IRequest<Result<User>>;