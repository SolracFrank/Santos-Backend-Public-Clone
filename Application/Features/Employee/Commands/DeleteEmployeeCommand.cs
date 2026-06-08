using MediatR;
using LanguageExt.Common;

namespace Application.Features.Employees;

public record DeleteEmployeeCommand(int Id) : IRequest<Result<Unit>>;