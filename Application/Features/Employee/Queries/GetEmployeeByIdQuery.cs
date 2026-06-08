using Domain.Entities;
using MediatR;
using LanguageExt.Common;

namespace Application.Features.Employees;

public record GetEmployeeByIdQuery(int Id) : IRequest<Result<Employee?>>;