using Application.Features.Employees;
using Domain.Entities.Dtos;
using EsApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EsApi.Controllers.v1;

public class EmployeesController : BaseApiController
{
    public EmployeesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Gets all employees.", Description = "Returns a list of employees.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of employees.", typeof(IEnumerable<EmployeeDto>))]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEmployeesQuery(), cancellationToken);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(Summary = "Gets a employee by id.", Description = "Returns the employee that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the requested employee.", typeof(EmployeeDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found.")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await SendAsync(new GetEmployeeByIdQuery(id), cancellationToken);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a employee.", Description = "Creates a new employee using the request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the created employee.", typeof(EmployeeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The request body failed validation.")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        return await SendAsync(command, cancellationToken);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Updates a employee.", Description = "Updates an existing employee using the provided id and request body.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Returns the updated employee.", typeof(EmployeeDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The route id does not match the request body or validation failed.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found.")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id) return BadRequest();

        return await SendAsync(command, cancellationToken);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(Summary = "Deletes a employee.", Description = "Deletes the employee that matches the provided id.")]
    [SwaggerResponse(StatusCodes.Status200OK, "The employee was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The employee was not found.")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await SendOkAsync(new DeleteEmployeeCommand(id), cancellationToken);
    }
}