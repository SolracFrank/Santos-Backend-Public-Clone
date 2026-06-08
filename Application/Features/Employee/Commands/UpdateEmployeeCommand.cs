using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Employees;

public record UpdateEmployeeCommand(int Id, int Idemployee, string Ename, string Elastname, string? Eaddress, string? Ecity, int? Ezip, string? Eprint, string? Ephone, string? Eemail, int? Idstate) : IRequest<Result<Employee>>;