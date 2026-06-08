using Domain.Entities;
using MediatR;
using System.Collections.Generic;
using LanguageExt.Common;

namespace Application.Features.Employees;

public record GetEmployeesQuery : IRequest<Result<List<Employee>>>;