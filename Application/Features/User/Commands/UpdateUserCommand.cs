using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Users;

public record UpdateUserCommand(int Id, int Idusers, string Ucustomer, string? Ubalance, DateOnly? Udate, int? Idaddress, int? Idterms, int? Idcustomer, int? Idrepsales, int? Idjobinfo) : IRequest<Result<User>>;